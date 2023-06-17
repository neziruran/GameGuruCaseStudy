using System;
using Cinemachine;
using UnityEngine;

namespace _Project.Task2.Gameplay.Level
{
    public class CameraRotationController : MonoBehaviour
    {
        private CinemachineVirtualCamera _cinemachineVirtualCamera;
        private float _biasValue = -180f;
        [SerializeField] private float rotateSpeed = 15f;
        private void Awake()
        {
            _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        }

        private void Update()
        {
            RotateCamera();
        }

        private void RotateCamera()
        {
            _biasValue += Time.deltaTime * rotateSpeed;
            _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.Value =
                _biasValue;
        }
    }
}