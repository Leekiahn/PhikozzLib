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
        [SerializeField] private AssetLabelReference _audioLabel;

        private Dictionary<string, AudioEntry> _bgmEntriesByID = new();
        private Dictionary<string, AudioEntry> _sfxEntriesByID = new();
        private Dictionary<string, AudioEntry> _uiEntriesByID = new();
        
        
        private TrackedPool<AudioPlayer> _pool;
        
        public void RegisterService()
        {
            ServiceLocator.Register<IAudioService>(this);
        }

        public async UniTask Load()
        {
        }
    }
}