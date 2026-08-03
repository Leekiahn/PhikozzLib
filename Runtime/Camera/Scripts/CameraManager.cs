using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace PhikozzLib
{
    [RequireComponent(typeof(Camera), typeof(CinemachineBrain), typeof(AudioListener))]
    public class CameraManager : SingletonScene<CameraManager>
    {
        private const int ActivePriority = 100;
        private const int InactivePriority = 0;

        private readonly Dictionary<string, CinemachineCamera> _cameraByKey = new();
        private CinemachineCamera _activeCamera;

        public event Action<CinemachineCamera> OnCameraChanged;

        public void RegisterCamera(string cameraKey, CinemachineCamera cam)
        {
            _cameraByKey.Add(cameraKey, cam);
        }

        public void UnregisterCamera(string cameraKey)
        {
            _cameraByKey.Remove(cameraKey);
        }

        public void SetCamera(string cameraKey)
        {
            if (_cameraByKey.TryGetValue(cameraKey, out var cam))
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

        public CinemachineCamera GetCamera(string cameraKey)
        {
            if (_cameraByKey.TryGetValue(cameraKey, out var cam))
            {
                return cam;
            }

            return null;
        }

        public CinemachineCamera GetActiveCamera()
        {
            return _activeCamera;
        }

        public bool IsCurrent(string cameraKey)
        {
            if (_cameraByKey.TryGetValue(cameraKey, out var cam))
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