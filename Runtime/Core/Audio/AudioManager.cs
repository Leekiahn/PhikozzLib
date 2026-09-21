using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;

namespace PhikozzLib
{
    public class AudioManager : MonoBehaviour, IAudioService, IServiceRegister, IServiceInit
    {
        private const float MIN_MIXER_VOLUME = -80f;

        [Title("Load Settings")]
        [SerializeField] private bool _loadByAddressableService;

        [ShowIf("_loadByAddressableService")]
        [SerializeField] private AssetLabelReference _audioClipLabel;

        [Title("Audio Mixer")]
        [InfoBox("AudioMixer에서 Master/BGM/SFX 그룹의 Volume을 Expose to script로 설정하고, 아래 파라미터 이름을 일치시켜야 합니다.")]
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private AudioMixerGroup _bgmMixerGroup;
        [SerializeField] private AudioMixerGroup _sfxMixerGroup;
        [SerializeField] private string _masterVolumeParameter = "Master";
        [SerializeField] private string _musicVolumeParameter = "Music";
        [SerializeField] private string _sfxVolumeParameter = "Sfx";

        [Title("Volume")]
        [SerializeField, Range(0f, 1f), OnValueChanged(nameof(HandleVolumeChanged))] private float _masterVolume = 1f;
        [SerializeField, Range(0f, 1f), OnValueChanged(nameof(HandleVolumeChanged))] private float _bgmVolume = 1f;
        [SerializeField, Range(0f, 1f), OnValueChanged(nameof(HandleVolumeChanged))] private float _sfxVolume = 1f;

        [Title("SFX Pool")]
        [SerializeField] private int _sfxPoolCapacity = 10;
        [SerializeField] private int _sfxPoolMaxSize = 20;

        private readonly Dictionary<string, AudioClip> _audioClips = new();

        private Transform _audioParent;
        private AudioSource _bgmSource;
        private TrackedPool<AudioSource> _sfxPool;
        private IAddressableService _addressableService;

        private void Awake()
        {
            _audioParent = transform;
            _bgmSource = CreateAudioSource("Audio_Bgm", _bgmMixerGroup);
            _sfxPool = CreateSfxPool();
            
            SetMixerVolume(_masterVolumeParameter, _masterVolume);
            SetMixerVolume(_musicVolumeParameter, _bgmVolume);
            SetMixerVolume(_sfxVolumeParameter, _sfxVolume);
        }

        private void OnValidate()
        {
            HandleVolumeChanged();
        }

        public void RegisterService()
        {
            ServiceLocator.Register<IAudioService>(this);
        }

        public void UnregisterService()
        {
            ServiceLocator.Unregister<IAudioService>();
        }

        public async UniTask InitAsync()
        {
            if (_loadByAddressableService)
            {
                _addressableService = ServiceLocator.Get<IAddressableService>();
                await PreloadAudioClipsByAddressableService();
            }
        }

        public void RegisterAudio(string key, AudioClip clip)
        {
            if (!_audioClips.ContainsKey(key))
            {
                _audioClips[key] = clip;
            }
        }

        public void UnregisterAudio(string key)
        {
            if (_audioClips.TryGetValue(key, out var clip))
            {
                if (_bgmSource.clip == clip)
                {
                    StopBgm();
                }

                _audioClips.Remove(key);
            }
        }

        [PropertySpace(SpaceBefore = 20f)]
        [Button(ButtonSizes.Medium, ButtonStyle.Box)]
        public AudioSource PlayBgm(string key)
        {
            if (_audioClips.TryGetValue(key, out var clip))
            {
                _bgmSource.Stop();
                _bgmSource.outputAudioMixerGroup = _bgmMixerGroup;
                _bgmSource.clip = clip;
                _bgmSource.volume = 1f;
                _bgmSource.loop = true;
                _bgmSource.Play();

                return _bgmSource;
            }

            return null;
        }

        [PropertySpace(SpaceBefore = 20f)]
        [Button(ButtonSizes.Medium, ButtonStyle.Box)]
        public void StopBgm()
        {
            _bgmSource.Stop();
            _bgmSource.clip = null;
        }

        [PropertySpace(SpaceBefore = 20f)]
        [Button(ButtonSizes.Medium, ButtonStyle.Box)]
        public AudioSource PlaySfx(string key, Vector3 position, float spatialBlend = 0f, float volume = 1f, float pitch = 1f)
        {
            if (_audioClips.TryGetValue(key, out var clip))
            {
                var audioSource = _sfxPool.Get();
                audioSource.outputAudioMixerGroup = _sfxMixerGroup;
                audioSource.transform.position = position;
                audioSource.clip = clip;
                audioSource.volume = Mathf.Clamp01(volume);
                audioSource.pitch = pitch;
                audioSource.spatialBlend = spatialBlend;
                audioSource.loop = false;
                audioSource.Play();

                ReleaseSfxAsync(audioSource).Forget();
                return audioSource;
            }
            return null;
        }

        [PropertySpace(SpaceBefore = 20f)]
        [Button(ButtonSizes.Medium, ButtonStyle.Box)]
        public AudioSource PlaySfxAttached(string key, Transform parent, float spatialBlend = 0f, float volume = 1f, float pitch = 1f)
        {
            if (_audioClips.TryGetValue(key, out var clip))
            {
                var audioSource = _sfxPool.Get();
                audioSource.outputAudioMixerGroup = _sfxMixerGroup;
                audioSource.transform.SetParent(parent, false);
                audioSource.transform.localPosition = parent.localPosition;
                audioSource.clip = clip;
                audioSource.volume = Mathf.Clamp01(volume);
                audioSource.pitch = pitch;
                audioSource.spatialBlend = spatialBlend;
                audioSource.loop = false;
                audioSource.Play();

                ReleaseSfxAsync(audioSource).Forget();
                return audioSource;
            }
            return null;
        }

        [PropertySpace(SpaceBefore = 20f)]
        [Button(ButtonSizes.Medium, ButtonStyle.Box)]
        public void SetMasterVolume(float volume)
        {
            _masterVolume = Mathf.Clamp01(volume);
            SetMixerVolume(_masterVolumeParameter, _masterVolume);
        }

        [PropertySpace(SpaceBefore = 20f)]
        [Button(ButtonSizes.Medium, ButtonStyle.Box)]
        public void SetBgmVolume(float volume)
        {
            _bgmVolume = Mathf.Clamp01(volume);
            SetMixerVolume(_musicVolumeParameter, _bgmVolume);
        }

        [PropertySpace(SpaceBefore = 20f)]
        [Button(ButtonSizes.Medium, ButtonStyle.Box)]
        public void SetSfxVolume(float volume)
        {
            _sfxVolume = Mathf.Clamp01(volume);
            SetMixerVolume(_sfxVolumeParameter, _sfxVolume);
        }




        private async UniTask PreloadAudioClipsByAddressableService()
        {
            await _addressableService.PreloadLocations<AudioClip>(_audioClipLabel.labelString);
            await _addressableService.PreloadAssets<AudioClip>(_audioClipLabel.labelString);

            var audioClips = _addressableService.GetAllWithKeys<AudioClip>(_audioClipLabel.labelString);

            foreach (var clip in audioClips)
            {
                RegisterAudio(clip.Key, clip.Value);
            }
        }

        private TrackedPool<AudioSource> CreateSfxPool()
        {
            return new TrackedPool<AudioSource>(
                onCreate: () => CreateAudioSource("Audio_Sfx", _sfxMixerGroup),
                onGet: source => source.gameObject.SetActive(true),
                onRelease: source =>
                {
                    source.Stop();
                    source.clip = null;
                    source.transform.SetParent(_audioParent);
                    source.gameObject.SetActive(false);
                },
                onDestroy: source => Destroy(source.gameObject),
                defaultCapacity: _sfxPoolCapacity,
                maxSize: _sfxPoolMaxSize
            );
        }

        private AudioSource CreateAudioSource(string objectName, AudioMixerGroup mixerGroup)
        {
            var audioObject = new GameObject(objectName);
            audioObject.transform.SetParent(_audioParent);

            var audioSource = audioObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.outputAudioMixerGroup = mixerGroup;
            return audioSource;
        }

        private async UniTaskVoid ReleaseSfxAsync(AudioSource audioSource)
        {
            await UniTask.WaitUntil(() => audioSource == null || !audioSource.isPlaying);

            if (audioSource != null)
            {
                _sfxPool.Release(audioSource);
            }
        }

        private void HandleVolumeChanged()
        {
            _masterVolume = Mathf.Clamp01(_masterVolume);
            _bgmVolume = Mathf.Clamp01(_bgmVolume);
            _sfxVolume = Mathf.Clamp01(_sfxVolume);

            if (_audioMixer != null)
            {
                SetMixerVolume(_masterVolumeParameter, _masterVolume);
                SetMixerVolume(_musicVolumeParameter, _bgmVolume);
                SetMixerVolume(_sfxVolumeParameter, _sfxVolume);
            }
        }

        private void SetMixerVolume(string parameterName, float volume)
        {
            _audioMixer.SetFloat(parameterName, ConvertNormalizedVolumeToDecibels(volume));
        }

        private float ConvertNormalizedVolumeToDecibels(float volume)
        {
            if (volume <= 0f)
            {
                return MIN_MIXER_VOLUME;
            }

            return Mathf.Log10(volume) * 20f;
        }
    }
}
