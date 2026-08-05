using UnityEngine;

namespace PhikozzLib
{
    public interface IAudioService
    {
        public void PlayBgm(string channelKey, int index);
        public void StopBgm();
        public void PauseBgm();
        public void ResumeBgm();
        public void PlayNextBgm();
        public void PlayPreviousBgm();
        public void SetBgmMultiplier(float volume = 1f, float pitch = 1f, bool instantly = true);
        
        
        public void PlaySfx(string soundName, Vector3 position = default, Transform attachToTransform = null);
        public void PlayUi(string soundName, Vector3 position = default, Transform attachToTransform = null);
        public void PlayOther(string soundName, Vector3 position = default, Transform attachToTransform = null);

        
        public void ControlTrack(eSoundTrackEventTypes type, eSoundTracks track, float volume = 1f);
        public void ControlAllTrack(eAllSoundControlEventTypes type);
        public void FadeTrack(eSoundTrackFadeEventModes mode, eSoundTracks track, float fadeDuration, float finalVolume, eFadeTrackTweenType fadeTween);
    }
}