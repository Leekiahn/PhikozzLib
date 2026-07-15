using UnityEngine;
using MoreMountains.Tools;

namespace PhikozzLib
{
    public class AudioManager : MonoBehaviour, IAudioService, IServiceRegister
    {
        [SerializeField] private SoundDatabase _soundDatabase;

        private int _currentPlaylistChannel;
        
        
        public void RegisterService()
        {
            ServiceLocator.Register<IAudioService>(this);
        }

        public void PlayPlaylist(int channel, int index = 0)
        {
            _currentPlaylistChannel = channel;
            MMPlaylistPlayIndexEvent.Trigger(_currentPlaylistChannel, index);
        }
        
        public void StopCurrentPlaylist()
        {
            MMPlaylistStopEvent.Trigger(_currentPlaylistChannel);
        }

        public void PauseCurrentPlaylist()
        {
            MMPlaylistPauseEvent.Trigger(_currentPlaylistChannel);
        }

        public void PlayNextPlaylistIndex()
        {
            MMPlaylistPlayNextEvent.Trigger(_currentPlaylistChannel);
        }

        public void PlayPreviousPlaylistIndex()
        {
            MMPlaylistPlayPreviousEvent.Trigger(_currentPlaylistChannel);
        }

        public void SetPlaylistMultiplier(float volume = 1f, float pitch = 1f, bool instantly = true)
        {
            MMPlaylistVolumeMultiplierEvent.Trigger(_currentPlaylistChannel, volume, instantly);
            MMPlaylistPitchMultiplierEvent.Trigger(_currentPlaylistChannel, pitch, instantly);
        }
        
        public void PlaySfx(string soundName, Vector3 position = default, Transform attachToTransform = null)
        {
            if (_soundDatabase.SfxSoundDataDic.TryGetValue(soundName, out var soundData))
            {
                if (attachToTransform != null)
                {
                    soundData.AttachToTransform = attachToTransform;
                    soundData.Play(attachToTransform.position);
                }
                else
                {
                    soundData.Play(position);
                }
            }
        }

        public void PlayUi(string soundName)
        {
            if (_soundDatabase.UiSoundDataDic.TryGetValue(soundName, out var soundData))
            {
                soundData.Play(Vector3.zero);
            }
        }

        public void ControlTrack(MMSoundManagerTrackEventTypes type, MMSoundManager.MMSoundManagerTracks track, float volume)
        {
            MMSoundManagerTrackEvent.Trigger(type, track, volume);
        }

        public void ControlAllTrack(MMSoundManagerAllSoundsControlEventTypes type)
        {
            MMSoundManagerAllSoundsControlEvent.Trigger(type);
        }
        
        public void FadeTrack(MMSoundManagerTrackFadeEvent.Modes mode, MMSoundManager.MMSoundManagerTracks track, float fadeDuration, float finalVolume, MMTweenType fadeTween)
        {
            MMSoundManagerTrackFadeEvent.Trigger(mode, track, fadeDuration, finalVolume, fadeTween);
        }
        
        // TODO:
        // 다른 AudioManager로 교체할 수 있도록 매개변수들에 있는 MMSoundManager들은 제거하고
        // 따로 enum 값을 만든다.
    }
}