using UnityEngine;
using Zenject;
using Cinemachine;

namespace _Project.Task2.Managers
{
    public class CameraManager : MonoBehaviour
    {
        [Inject] 
        private EventTriggers _eventTriggers;
        
        [SerializeField] private CinemachineVirtualCamera gameplayCamera;
        [SerializeField] private CinemachineVirtualCamera finishCamera;

        private void OnEnable()
        {
            _eventTriggers.OnCameraFinish += PlayerWin;
            _eventTriggers.OnCameraStop += FollowStop;
            _eventTriggers.OnCameraStart += StartCamera;
        }        

        private void OnDisable()
        {
            _eventTriggers.OnCameraFinish -= PlayerWin;
            _eventTriggers.OnCameraStop -= FollowStop;
            _eventTriggers.OnCameraStart -= StartCamera;
        }

        private void PlayerWin()
        {
            finishCamera.Priority = gameplayCamera.Priority+1;
        }

        private void StartCamera(Transform target)
        {
            gameplayCamera.Follow = target;
            finishCamera.Follow = target;
            finishCamera.Priority = gameplayCamera.Priority-1 ;
            finishCamera.m_LookAt = target;

        }

        private void FollowStop()
        {
            gameplayCamera.Follow = null;
        }
    }
}