using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace PhikozzLib
{
    [RequireComponent(typeof(Camera), typeof(CinemachineBrain), typeof(AudioListener))]
    public class CameraManager : MonoBehaviour, ICameraService, IServiceRegister
    {
        private CinemachineBrain _cinemachineBrain;

        private readonly Dictionary<string, CameraRegister> _camerasById = new Dictionary<string, CameraRegister>();
        
        private CameraRegister _currentCamera;

        private void Awake()
        {
            _cinemachineBrain = GetComponent<CinemachineBrain>();
        }

        public void RegisterService()
        {
            ServiceLocator.Register<ICameraService>(this);
        }

        public void RegisterCamera(string id, CameraRegister cameraRegister)
        {
            _camerasById.TryAdd(id, cameraRegister);
        }

        public void UnRegisterCamera(string id)
        {
            _camerasById.Remove(id);
        }

        public void SetCamera(string id)
        {
            if (_camerasById.TryGetValue(id, out var cameraRegister))
            {
                _currentCamera = cameraRegister;
                _currentCamera.CinemachineCamera.Priority = 0;
                cameraRegister.CinemachineCamera.Priority = 10;
            }
        }
    }
}