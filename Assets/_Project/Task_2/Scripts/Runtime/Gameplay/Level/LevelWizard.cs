using _Project.Task2.Managers;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace _Project.Task2.Gameplay.Level
{
    public enum LevelDifficulty
    {
        Easy, 
        Medium,
        Hard
    }
    
    public class LevelWizard : MonoBehaviour
    {

        [Inject] 
        private EventTriggers _eventTriggers;
        
        [Inject]
        private GameManager _gameManager;
        
        [Inject]
        private LevelManager _levelManager;

        [SerializeField] private GameObject startBlock;
        [SerializeField] private GameObject block;      
        [SerializeField] private GameObject player;
        [SerializeField] private Transform playerPos;
        [SerializeField] private Material[] blockColors;

        [SerializeField] private int levelLength;
      
        [SerializeField] private LevelDifficulty levelDifficulty;

        private AudioSource _audioSource;
        private GameObject _lastBlock;
        private GameObject _currentBlock;
        private GameObject _lastPlayer;

        private int _goodClicks;
        private int _blockCount;
        private float _blockMoveSpeed;
        private bool _lastCut;

        private void Awake()
        {
            InitStackPieces();
            _eventTriggers.OnGetLastBlockPosition?.Invoke(_lastBlock);
            InitGameLevel();
        }

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            SetLevelEndPosition();
        }

        private void SetLevelEndPosition()
        {
            float endPos = _lastPlayer.transform.position.z + ((levelLength + 1) * block.transform.localScale.z);
            _eventTriggers.OnProgressBarSet?.Invoke(_lastPlayer, endPos,_levelManager.CurrentLevel);
        }

        private void OnEnable()
        {
            _eventTriggers.OnGetFirstBlock += FirstBlock;
        }
        private void OnDisable()
        {
            _eventTriggers.OnGetFirstBlock -= FirstBlock;
        }

        private void InitStackPieces()
        {
            _lastBlock = startBlock;
            _lastPlayer = Instantiate(player);
            _lastPlayer.transform.position = playerPos.position;
            _lastPlayer.transform.rotation = playerPos.rotation;
        }

        private void InitGameLevel()
        {
            float playerInitSpeed = 1;
            playerInitSpeed = SetDifficultyMode(playerInitSpeed);
            _eventTriggers.OnSetPlayerSpeed?.Invoke(playerInitSpeed);
        }

        private float SetDifficultyMode(float playerSpeed)
        {
            switch (levelDifficulty)
            {
                case LevelDifficulty.Easy:
                    _blockMoveSpeed = 4;
                    playerSpeed = 1.4f;
                    break;
                case LevelDifficulty.Medium:
                    _blockMoveSpeed = 2.7f;
                    playerSpeed = 2f;
                    break;
                case LevelDifficulty.Hard:
                    _blockMoveSpeed = 1.85f;
                    playerSpeed = 2.8f;
                    break;
            }

            return playerSpeed;
        }

        private void FirstBlock()
        {

            NewBlockSpawn(startBlock.transform.localScale, startBlock.transform.position);
        }

        private void Update()
        {
            bool onTouch = Input.GetMouseButtonDown(0);
            bool isGameStarted = _gameManager.GetGameState() == GameState.Play;
            bool hasCooldown = _lastCut;
            if ((onTouch) && (isGameStarted || hasCooldown))
            {

                CutBlock();
            }
        }
        void CutBlock()
        {

            GameObject cutBlock = Instantiate(block, transform);
            cutBlock.GetComponent<MeshRenderer>().material = _currentBlock.GetComponent<MeshRenderer>().material;

            cutBlock.name = "CutPart";
            Vector3 targetPos = _lastBlock.transform.position;
            Vector3 blockPos = _currentBlock.transform.position;
            Vector3 originalScale = _currentBlock.transform.localScale;
            cutBlock.transform.position = new Vector3((targetPos.x + blockPos.x) / 2f, blockPos.y, blockPos.z);
            cutBlock.transform.localScale = new Vector3(_currentBlock.transform.localScale.x - Mathf.Abs(_lastBlock.transform.position.x - _currentBlock.transform.position.x), _currentBlock.transform.localScale.y, _currentBlock.transform.localScale.z);

            var isGoodClick = cutBlock.transform.localScale.x / _currentBlock.transform.localScale.x > .9f;
            if (isGoodClick)
            {
                _goodClicks++;
                _audioSource.pitch = 0.5f + (_goodClicks * .1f);
            }
            else
            {
                _goodClicks = 0;
                _audioSource.pitch = 0.5f;

            }
            
            PlayNote();
            var factor = SetFactor(blockPos, targetPos);
            SetCurrentBlock(targetPos, blockPos, originalScale, factor, cutBlock);
            DisableRigidbody(_currentBlock);
            DOTween.Kill(_currentBlock.transform);
            
            bool failThreshold = cutBlock.transform.localScale.x > .05f;
            if (failThreshold)
            {

                if (_blockCount < levelLength)
                {
                    NewBlockSpawn(cutBlock.transform.localScale, cutBlock.transform.position);
                }
                else
                {
                    _lastCut = false;
                    _eventTriggers.OnGameStop?.Invoke();
                }
                _lastBlock = cutBlock;
                _eventTriggers.OnGetLastBlockPosition?.Invoke(_lastBlock);
                CreateFakeLevel(cutBlock);
            }
            else
            {

                _eventTriggers.OnGameStop?.Invoke();
                cutBlock.transform.position = blockPos;
                cutBlock.transform.localScale = _lastBlock.transform.localScale;
                DisableRigidbody(cutBlock);
                Destroy(_currentBlock);
            }
        
        }

        private void PlayNote()
        {
            _audioSource.Stop();
            _audioSource.Play();
        }

        private static float SetFactor(Vector3 blockPos, Vector3 targetPos)
        {
            float factor = 0;
            if (blockPos.x - targetPos.x > 0)
            {
                factor = 1;
            }
            else
            {
                factor = -1;
            }

            return factor;
        }

        private void SetCurrentBlock(Vector3 targetPos, Vector3 blockPos, Vector3 originalScale, float factor,
            GameObject cutBlock)
        {
            _currentBlock.transform.position = new Vector3((targetPos.x + blockPos.x) / 2f + originalScale.x * factor / 2f,
                blockPos.y, blockPos.z);
            _currentBlock.transform.localScale =
                new Vector3((_lastBlock.transform.localScale.x - cutBlock.transform.localScale.x),
                    _lastBlock.transform.localScale.y, _lastBlock.transform.localScale.z);
        }

        private void DisableRigidbody(GameObject cutBlock)
        {
            if (cutBlock.GetComponent<Rigidbody>()) return;
            
            cutBlock.AddComponent<Rigidbody>().mass = 100f;
            cutBlock.GetComponent<Collider>().enabled = false;
        }

        private static void CreateFakeLevel(GameObject cutBlock)
        {
            GameObject fakeLevel = Instantiate(cutBlock, cutBlock.transform.parent);
            fakeLevel.tag = "Untagged";
            fakeLevel.GetComponent<Collider>().enabled = false;
            fakeLevel.transform.DOScale(fakeLevel.transform.localScale * 1.15f, .3f).SetLoops(2, LoopType.Yoyo).OnComplete(
                () => { Destroy(fakeLevel); }
            );
            fakeLevel.GetComponent<MeshRenderer>().material.color = Color.white;
            fakeLevel.GetComponent<MeshRenderer>().material.DOFade(0, .5f);
        }

        void NewBlockSpawn(Vector3 scale, Vector3 position)
        {

            if (_blockCount < levelLength)
            {
                _blockCount++;

                position.z = startBlock.transform.position.z + (_blockCount * startBlock.transform.localScale.z);
                if (_blockCount % 2 == 1)
                {
                    position.x = startBlock.transform.position.x + (startBlock.transform.localScale.x * 1.5f);
                }
                else
                {
                    position.x = startBlock.transform.position.x - (startBlock.transform.localScale.x * 1.5f);
                }
                var newBlock = InstantiateNewBlock(scale, position);
                SetMovement(newBlock);
                _currentBlock = newBlock;
            }
            else
            {
                _lastCut = true;
            }
        }

        private GameObject InstantiateNewBlock(Vector3 scale, Vector3 pos)
        {
            GameObject newBlock = Instantiate(block, pos, block.transform.rotation, transform);
            newBlock.GetComponent<MeshRenderer>().material = blockColors[Random.Range(0, blockColors.Length - 1)];
            newBlock.transform.localScale = scale;
            return newBlock;
        }

        private void SetMovement(GameObject newBlock)
        {
            if (_blockCount % 2 == 0)
            {
                newBlock.transform.DOMoveX(startBlock.transform.position.x + (startBlock.transform.localScale.x * 1.5f),
                        _blockMoveSpeed)
                    .SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            }
            else
            {
                newBlock.transform.DOMoveX(startBlock.transform.position.x - (startBlock.transform.localScale.x * 1.5f),
                        _blockMoveSpeed)
                    .SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            }
        }
    }

}
