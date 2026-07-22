using Unity.Cinemachine;
using UnityEngine;

namespace PhikozzLib
{
    [RequireComponent(typeof(CinemachineCamera))]
    public class CameraRegister : MonoBehaviour
    {
        [SerializeField] private string _id;
        
        public string Id => _id;
        public CinemachineCamera CinemachineCamera { get; private set; }
        
        private void Awake()
        {
            CinemachineCamera = GetComponent<CinemachineCamera>();
            Core.Camera.RegisterCamera(_id, this);
        }
        
        private void OnDestroy()
        {
            Core.Camera.UnRegisterCamera(_id);
        }
    }
}

