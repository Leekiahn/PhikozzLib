using Unity.Cinemachine;
using UnityEngine;

namespace PhikozzLib
{
    public class CameraLoader : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera _camera;
        [SerializeField] private eCameraType _cameraType;

        private void Awake()
        {
            if (_camera == null)
            {
                _camera = GetComponent<CinemachineCamera>();
            }
        }

        private void OnEnable()
        {
            CameraManager.Instance.RegisterCamera(_cameraType, _camera);
        }

        private void OnDisable()
        {
            CameraManager.Instance.UnregisterCamera(_cameraType);
        }

        private void OnDestroy()
        {
            CameraManager.Instance.UnregisterCamera(_cameraType);
        }
    }
}
