using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace PhikozzLib
{
    [RequireComponent(typeof(Camera), typeof(CinemachineBrain), typeof(AudioListener))]
    public class CameraManager : SingletonScene<CameraManager>
    {
        [Serializable]
        private class CameraData
        {
            public eCameraType CameraType;
            public CinemachineCamera Camera;
        }
        
        private const int ActivePriority = 100;
        private const int InactivePriority = 0;

        [SerializeField] private List<CameraData> _cameras;
        private readonly Dictionary<eCameraType, CinemachineCamera> _cameraDic = new();

        private CinemachineBrain _cinemaBrain;
        private CinemachineCamera _activeCamera;

        public event Action<CinemachineCamera> OnCameraChanged;

        protected override void Awake()
        {
            base.Awake();
            _cinemaBrain = GetComponent<CinemachineBrain>();

            foreach (var cam in _cameras)
            {
                _cameraDic[cam.CameraType] = cam.Camera;
            }
        }


        public void RegisterCamera(eCameraType cameraType, CinemachineCamera cam)
        {
            _cameraDic[cameraType] = cam;
        }

        public void UnregisterCamera(eCameraType cameraType)
        {
            _cameraDic.Remove(cameraType);
        }


        public void SetCamera(eCameraType cameraType)
        {
            if (_cameraDic.TryGetValue(cameraType, out var cam))
            {
                if (_activeCamera != null)
                {
                    _activeCamera.Priority = InactivePriority;
                }

                cam.Priority = ActivePriority;
                _activeCamera = cam;

                OnCameraChanged?.Invoke(_activeCamera);
            }
        }

        public CinemachineCamera GetCamera(eCameraType cameraType)
        {
            if (_cameraDic.TryGetValue(cameraType, out var cam))
            {
                return cam;
            }

            return null;
        }

        public CinemachineCamera GetActiveCamera()
        {
            return _activeCamera;
        }

        public bool IsCurrent(eCameraType cameraType)
        {
            if (_cameraDic.TryGetValue(cameraType, out var cam))
            {
                return _activeCamera == cam;
            }

            return false;
        }
        
        public bool IsCurrent(CinemachineCamera cam)
        {
            return _activeCamera == cam;
        }
    }
}