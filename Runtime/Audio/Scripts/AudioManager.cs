using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace PhikozzLib
{
    public class AudioManager : MonoBehaviour, IAudioService, IServiceRegister
    {
        [SerializeField] private AssetReferenceT<AudioDatabase> _audioDatabaseReference;
        [SerializeField] private AssetLabelReference _audioLabelReference;
        [SerializeField] private AudioPlayer _audioPlayer;

        private Dictionary<string, AudioEntry> _bgmEntriesByID = new();
        private Dictionary<string, AudioClip> _bgmClipsByID = new();
        private Dictionary<string, AudioEntry> _sfxEntriesByID = new();
        private Dictionary<string, AudioClip> _sfxClipsByID = new();
        private Dictionary<string, AudioEntry> _uiEntriesByID = new();
        private Dictionary<string, AudioClip> _uiClipsByID = new();
        
        
        private TrackedPool<AudioPlayer> _pool;
        private AudioDatabase _audioDatabase;
        
        public void RegisterService()
        {
            ServiceLocator.Register<IAudioService>(this);
        }

        public async UniTask Load()
        {
            await Core.Addressable.Load<AudioDatabase>(_audioLabelReference.labelString);
            _audioDatabase = Core.Addressable.Get<AudioDatabase>(_audioLabelReference.labelString, _audioDatabaseReference.AssetGUID);
            
            await Core.Addressable.Load<AudioClip>(_audioLabelReference.labelString);
            
            foreach (var bgmEntry in _audioDatabase.BgmDatabase.Entries)
            {
                _bgmEntriesByID[bgmEntry.ID] = bgmEntry;
                _bgmClipsByID[bgmEntry.ID] = Core.Addressable.Get<AudioClip>(_audioLabelReference.labelString, bgmEntry.ID);
            }

            foreach (var sfxEntry in _audioDatabase.SfxDatabase.Entries)
            {
                _sfxEntriesByID[sfxEntry.ID] = sfxEntry;
                _sfxClipsByID[sfxEntry.ID] = Core.Addressable.Get<AudioClip>(_audioLabelReference.labelString, sfxEntry.ID);
            }

            foreach (var uiEntry in _audioDatabase.UiDatabase.Entries)
            {
                _uiEntriesByID[uiEntry.ID] = uiEntry;
                _uiClipsByID[uiEntry.ID] = Core.Addressable.Get<AudioClip>(_audioLabelReference.labelString, uiEntry.ID);
            }

            _pool = new TrackedPool<AudioPlayer>(
                onCreate:() =>
                {
                    var player = Instantiate(_audioPlayer, transform);
                    return player;
                },
                onGet:(player) =>
                {
                    player.gameObject.SetActive(true);
                },
                onRelease:(player) =>
                {
                    player.Stop();
                    player.gameObject.SetActive(false);
                },
                onDestroy:(player) =>
                {
                    Destroy(player.gameObject);
                }
                );
        }

        public void PlayBgm(string id)
        {
            if (_bgmEntriesByID.TryGetValue(id, out var entry))
            {
                var player = _pool.Get();
                player.Play(_bgmClipsByID[entry.ID], entry.Volume, entry.Pitch, entry.Loop);
            }
        }

        public void PlaySfx(string id)
        {
            if (_sfxEntriesByID.TryGetValue(id, out var entry))
            {
                var player = _pool.Get();
                player.Play(_sfxClipsByID[entry.ID], entry.Volume, entry.Pitch, entry.Loop);
            }
        }

        public void PlayUi(string id)
        {
            if (_uiEntriesByID.TryGetValue(id, out var entry))
            {
                var player = _pool.Get();
                player.Play(_uiClipsByID[entry.ID], entry.Volume, entry.Pitch, entry.Loop);
            }
        }
    }
}