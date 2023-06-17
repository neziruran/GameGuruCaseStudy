using System.Collections;
using _Project.Task2.Managers;
using UnityEngine;
using Zenject;

namespace _Project.Task2.Gameplay.Player
{  public enum PlayerState
    {
        Idle, 
        Run, 
        Fall, 
        Win
    }

    public class PlayerController : MonoBehaviour
    {
        private const float TimeMultiplier = 3;

        
        [Inject] private GameManager _gameManager;

        [Inject] private EventTriggers _gameEvents;


        [SerializeField] private Animator animator;
        [SerializeField] private RuntimeAnimatorController idleAnim, runAnim, fallAnim, danceAnim;

        
        
        private float _playerSpeed;
        private GameObject _lastPlatform;
        private Rigidbody _rb;
        private PlayerState _playerState = PlayerState.Idle;

        private void OnEnable()
        {
            _gameEvents.OnGetLastBlockPosition += GetLastBlockPos;
            _gameEvents.OnSetPlayerSpeed += SetPlayerSpeed;
        }


        private void OnDisable()
        {
            _gameEvents.OnGetLastBlockPosition -= GetLastBlockPos;
            _gameEvents.OnSetPlayerSpeed -= SetPlayerSpeed;
        }

        private void SetPlayerSpeed(float speed)
        {
            _playerSpeed = speed;
        }

        private void GetLastBlockPos(GameObject lastBlock)
        {
            _lastPlatform = lastBlock;
        }

        private void Start()
        {
            _gameEvents.OnCameraStart?.Invoke(gameObject.transform);
            _gameEvents.OnGetLastPlayer?.Invoke(gameObject);
            _rb = GetComponent<Rigidbody>();
            OnGameStart();
        }

        private void OnGameStart()
        {
            _playerState = PlayerState.Idle;
            StartCoroutine(OnWaitForStart());
            ChangeAnim();
        }

        void FixedUpdate()
        {
            if (_playerState != PlayerState.Run) return;
            
            
            var selfTransform = transform;
            Vector3 drawPosition = selfTransform.position + Vector3.up + (Vector3.forward * .1f);
            var ray = new Ray(drawPosition, -selfTransform.up);

            if (Physics.Raycast(ray, out var hit, 10))
            {
                MoveForward();

                bool isFinish = hit.collider.CompareTag("Finish");
                if (isFinish)
                {

                    _playerState = PlayerState.Win;
                    ChangeAnim();
                    StartCoroutine(DelayWin());
                }
            }
            else
            {
                _playerSpeed = 0;
                Debug.DrawRay(drawPosition, -transform.up * 2, Color.blue);
                _playerState = PlayerState.Fall;
                _gameEvents.OnGameStop();

                foreach (var item in GetComponents<Collider>())
                {
                    item.isTrigger = true;
                }

                ChangeAnim();
                StartCoroutine(DelayDeath());
            }
        }

        private void MoveForward()
        {
            _rb.velocity = (Vector3.forward * _playerSpeed);
            var selfPosition = _rb.transform.position;
            selfPosition = Vector3.Lerp(selfPosition,
                new Vector3(_lastPlatform.transform.position.x, selfPosition.y, selfPosition.z),
                TimeMultiplier * Time.deltaTime);
            _rb.transform.position = selfPosition;
        }

        void ChangeAnim()
        {
            animator.runtimeAnimatorController = _playerState switch
            {
                PlayerState.Idle => idleAnim,
                PlayerState.Run => runAnim,
                PlayerState.Fall => fallAnim,
                PlayerState.Win => danceAnim,
                _ => animator.runtimeAnimatorController
            };
        }

        IEnumerator DelayWin()
        {
            _gameEvents.OnCameraFinish?.Invoke();
            yield return new WaitForSeconds(2);
            _gameEvents.OnPlayerWin?.Invoke();
        }

        IEnumerator DelayDeath()
        {
            yield return new WaitForSeconds(2);
            _gameEvents.OnPlayerDeath?.Invoke();
            _gameEvents.OnCameraStop?.Invoke();
        }

        IEnumerator OnWaitForStart()
        {
            while (_gameManager.GetGameState() != GameState.Play) yield return null;

            _playerState = PlayerState.Run;

            ChangeAnim();
        }
    }
}