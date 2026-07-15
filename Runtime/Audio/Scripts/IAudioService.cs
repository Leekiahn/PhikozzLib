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
        public void ControlAllTrack(eAllSoundControlEventTypes type);
        public void FadeTrack(eSoundTrackFadeEventModes mode, eSoundTracks track, float fadeDuration, float finalVolume, eFadeTrackTweenType fadeTween);
    }
}