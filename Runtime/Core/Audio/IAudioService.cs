using UnityEngine;

namespace PhikozzLib
{
    public interface IAudioService
    {
        void PlayBgm(string channelKey, int index);
        void StopBgm();
        void PauseBgm();
        void ResumeBgm();
        void PlayNextBgm();
        void PlayPreviousBgm();
        void SetBgmMultiplier(float volume = 1f, float pitch = 1f, bool instantly = true);


        void Play(string groupName, string soundName, Vector3 position = default, Transform attachToTransform = null);
        void Stop(string groupName, string soundName, Vector3 position = default, Transform attachToTransform = null);

        
        void ControlTrack(eSoundTrackEventTypes type, eSoundTracks track, float volume = 1f);
        void ControlAllTrack(eAllSoundControlEventTypes type);
        void FadeTrack(eSoundTrackFadeEventModes mode, eSoundTracks track, float fadeDuration, float finalVolume, eFadeTrackTweenType fadeTween);
    }
}