using System.Collections;
using UnityEngine;
using Zenject;

namespace _Project.Task2.Managers
{
    public enum GameState
    {
        Start,
        Play,
        Stop,
        Fail,
        Win
    }


    public class GameManager : MonoBehaviour
    {
        [Inject] private EventTriggers _gameEvents;

        private GameState _gameState;

        private void OnEnable()
        {
            _gameEvents.OnPlayerWin += PlayerWin;
            _gameEvents.OnPlayerDeath += PlayerDeath;
            _gameEvents.OnGameStart += OnGameStart;
            _gameEvents.OnGameStop += OnGameStop;
        }


        private void OnDisable()
        {
            _gameEvents.OnPlayerWin -= PlayerWin;
            _gameEvents.OnPlayerDeath -= PlayerDeath;
            _gameEvents.OnGameStart -= OnGameStart;
            _gameEvents.OnGameStop -= OnGameStop;
        }

        
        
        private void Start()
        {
            StartCoroutine(WaitForPlayerInput());
            OnChangeGameState(GameState.Start);
        }

        private void PlayerDeath()
        {
            OnChangeGameState(GameState.Fail);
        }

        private void PlayerWin()
        {
            OnChangeGameState(GameState.Win);
        }

        public GameState GetGameState()
        {
            return _gameState;
        }
        
        private void OnChangeGameState(GameState gameState)
        {
            _gameState = gameState;
            _gameEvents.OnGameStateSwitch?.Invoke(_gameState);
        }

        private void OnGameStart()
        {
            OnChangeGameState(GameState.Start);
            StartCoroutine(WaitForPlayerInput());
        }

        private void OnGameStop()
        {
            OnChangeGameState(GameState.Stop);
        }

        IEnumerator WaitForPlayerInput()
        {
            while (_gameState != GameState.Play)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    OnChangeGameState(GameState.Play);
                    _gameEvents.OnGetFirstBlock?.Invoke();
                }

                yield return null;
            }
        }
    }
}