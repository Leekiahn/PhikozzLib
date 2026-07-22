using Unity.Cinemachine;
using UnityEngine;

namespace PhikozzLib
{
    public interface ICameraService
    {
        public void RegisterCamera(string id, CameraRegister cameraRegister);
        public void UnRegisterCamera(string id);
        public void SetCamera(string id);

    }
}

