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
        
        public void PlaySfx(string soundName, Vector3 position = default)
        {
            if (_soundDatabase.SfxSoundDataDic.TryGetValue(soundName, out var soundData))
            {
                soundData.Play(position);
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
            MMSoundManagerTrackFadeEvent.Trigger(mode, track, finalVolume, fadeDuration, fadeTween);
        }
    }
}