using Unity.Cinemachine;
using UnityEngine;

namespace PhikozzLib
{
    public abstract class BaseCameraLoader : MonoBehaviour
    {
        protected abstract string CameraKey { get; }
        private CinemachineCamera _camera;

        private void Awake()
        {
            _camera = GetComponent<CinemachineCamera>();
        }

        private void Start()
        {
            CameraManager.Instance.RegisterCamera(CameraKey, _camera);
        }

        private void OnDisable()
        {
            CameraManager.Instance.UnregisterCamera(CameraKey);
        }

        private void OnDestroy()
        {
            CameraManager.Instance.UnregisterCamera(CameraKey);
        }
    }
}