using UnityEngine;

namespace Project.Task_1
{
    public class CameraManager : MonoBehaviour
    {
        [HideInInspector] public Camera mainCamera;
        private const float RefRatio = (float)1080 / 1920;
        private void Awake()
        {
            mainCamera = Camera.main;
        }

        //set camera size
        public void SetOrthographicSize(float size)
        {
            var currentRatio = (float)Screen.width / Screen.height;
            var multiplier = currentRatio / RefRatio;
            mainCamera.orthographicSize = (size)/multiplier;
        }
    }
}