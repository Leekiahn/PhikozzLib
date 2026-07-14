using UnityEngine;
using MoreMountains.Tools;

namespace PhikozzLib
{
    public interface IAudioService
    {
        public void PlayPlaylist(int channel, int index = 0);
        public void StopCurrentPlaylist();
        public void PauseCurrentPlaylist();
        public void PlayNextPlaylistIndex();
        public void PlayPreviousPlaylistIndex();
        public void SetPlaylistMultiplier(float volume = 1f, float pitch = 1f, bool instantly = false);
        public void PlaySfx(string soundName, Vector3 position = default, Transform attachToTransform = null);
        public void PlayUi(string soundName);
        public void ControlTrack(MMSoundManagerTrackEventTypes type, MMSoundManager.MMSoundManagerTracks track, float volume);
        public void ControlAllTrack(MMSoundManagerAllSoundsControlEventTypes type);
        public void FadeTrack(MMSoundManagerTrackFadeEvent.Modes mode, MMSoundManager.MMSoundManagerTracks track, float fadeDuration, float finalVolume, MMTweenType fadeTween);
    }
}