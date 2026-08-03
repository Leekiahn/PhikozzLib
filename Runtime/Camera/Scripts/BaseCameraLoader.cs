using Unity.Cinemachine;
using UnityEngine;

namespace PhikozzLib
{
    public abstract class BaseCameraLoader : MonoBehaviour
    {
        protected abstract eCameraType CameraType { get; }
        private CinemachineCamera _camera;

        private void Awake()
        {
            _camera = GetComponent<CinemachineCamera>();
        }

        private void Start()
        {
            CameraManager.Instance.RegisterCamera(CameraType, _camera);
        }

        private void OnDisable()
        {
            CameraManager.Instance.UnregisterCamera(CameraType);
        }

        private void OnDestroy()
        {
            CameraManager.Instance.UnregisterCamera(CameraType);
        }
    }
}