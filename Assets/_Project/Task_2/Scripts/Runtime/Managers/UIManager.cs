using TMPro;
using UnityEngine;
using Zenject;

namespace _Project.Task2.Managers
{
    public class UIManager : MonoBehaviour
    {
         [Inject] private EventTriggers _gameEvents;

        
        [SerializeField] private GameObject winPanel;
        [SerializeField] private GameObject failPanel;
        [SerializeField] private GameObject tapToPlayPanel;
        [SerializeField] private GameObject levelSliderPanel;
        

        private void OnEnable()
        {
            _gameEvents.OnGameStart += StartGame;
            _gameEvents.OnPlayerWin += PlayerWin;
            _gameEvents.OnPlayerDeath += PlayerDeath;
            _gameEvents.OnGetFirstBlock += FirstBlock;
            _gameEvents.OnGameStop += GameStop;

        }    

        private void OnDisable()
        {
            _gameEvents.OnGameStart -= StartGame;
            _gameEvents.OnPlayerWin -= PlayerWin;
            _gameEvents.OnPlayerDeath -= PlayerDeath;
            _gameEvents.OnGetFirstBlock -= FirstBlock;
            _gameEvents.OnGameStop -= GameStop;

        }
        private void GameStop()
        {
            levelSliderPanel.SetActive(false);
        }
        private void FirstBlock()
        {
            tapToPlayPanel.SetActive(false);
        }

        private void StartGame()
        {
            tapToPlayPanel.SetActive(true);
            levelSliderPanel.SetActive(true);
        }
        
        private void PlayerDeath()
        {
            levelSliderPanel.SetActive(false);
            failPanel.SetActive(true);
        }

        private void PlayerWin()
        {
          
            winPanel.SetActive(true);
        }
    }
}