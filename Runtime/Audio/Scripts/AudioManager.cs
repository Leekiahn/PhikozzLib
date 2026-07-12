using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace PhikozzLib
{
    public class AudioManager : MonoBehaviour, IAudioService, IServiceRegister
    {
        [SerializeField] private AudioDatabase _audioDatabase;
        [SerializeField] private AudioPlayer _audioPlayer;
        
        private TrackedPool<AudioPlayer> _pool;
        
        public void RegisterService()
        {
            ServiceLocator.Register<IAudioService>(this);
        }
    }
}