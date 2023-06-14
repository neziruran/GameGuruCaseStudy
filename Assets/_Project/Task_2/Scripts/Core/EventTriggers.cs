using UnityEngine;
using UnityEngine.Events;
using _Project.Task2.Managers;

namespace _Project.Task2.Managers
{
    public class EventTriggers
    {
        public UnityAction<GameState> OnGameStateSwitch;

        public UnityAction OnGameStart;
        public UnityAction OnGameStop;


        public UnityAction OnNextLevel;
        public UnityAction OnRestartLevel;

        public UnityAction OnPlayerWin;
        public UnityAction OnPlayerDeath;
        public UnityAction<GameObject> OnGetLastPlayer;
        public UnityAction<float> OnSetPlayerSpeed;

        public UnityAction OnCameraFinish;
        public UnityAction OnCameraStop;
        public UnityAction<Transform> OnCameraStart;

        public UnityAction<GameObject> OnGetLastBlockPosition;
        public UnityAction OnGetFirstBlock;


        public UnityAction<int> OnTextSpawn;

        public UnityAction<GameObject, float, int> OnProgressBarSet;

    }
}