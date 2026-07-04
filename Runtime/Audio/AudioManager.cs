using UnityEngine;
using UnityEngine.Audio;

namespace PhikozzLib
{
    public class AudioManager : MonoBehaviour, IAudioService, IServiceRegister
    {
        public void RegisterService()
        {
            ServiceLocator.Register<IAudioService>(this);
        }
    }
}