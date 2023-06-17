using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace _Project.Task2.Managers
{
    public class LevelManager : MonoBehaviour
    {
        [Inject] private EventTriggers _eventTriggers;
    
          
        [SerializeField] private GameObject[] levels;
        
        
        private GameObject _previousLevel;
        private GameObject _lastGameplayPlayer;


        private const string StartLevelStringKey = "currentLevel";
        
        public int CurrentLevel { get; private set; }


        private void Awake()
        {
            LoadLevel();
        }

        private void LoadLevel()
        {
            if (PlayerPrefs.GetInt(StartLevelStringKey) == 0)
            {
                PlayerPrefs.SetInt(StartLevelStringKey, 1);
            }

            CurrentLevel = PlayerPrefs.GetInt(StartLevelStringKey);
        }


        private void Start()
        {
            SetLevel();
        }
        
        
        private void OnEnable()
        {
            _eventTriggers.OnNextLevel += GetNextLevel;
            _eventTriggers.OnRestartLevel += RestartLevel;
            _eventTriggers.OnGetLastPlayer += GetLastPlayer;
              
        }
        private void OnDisable()
        {
            _eventTriggers.OnNextLevel -= GetNextLevel;
            _eventTriggers.OnRestartLevel -= RestartLevel;
            _eventTriggers.OnGetLastPlayer -= GetLastPlayer;
            
        }
    
        private void GetLastPlayer(GameObject player)
        {
            _lastGameplayPlayer = player;
        }
    
        private void GetNextLevel()
        {
            Destroy(_previousLevel);
            Destroy(_lastGameplayPlayer);
            CurrentLevel++;
            if (CurrentLevel > levels.Length)
            {
                CurrentLevel = 1;
            }
            PlayerPrefs.SetInt(StartLevelStringKey, CurrentLevel);
    
            SetLevel();
        }
          
        private void RestartLevel()
        {
    
                
            Destroy(_previousLevel);
            Destroy(_lastGameplayPlayer);
    
            SetLevel();
            
        }

          
        private void SetLevel()
        {
            _previousLevel = Instantiate(levels[CurrentLevel - 1]);
            _eventTriggers.OnGameStart?.Invoke();
    
        }
    }

}
