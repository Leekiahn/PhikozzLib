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

        public void ControlTrack(eSoundTrackEventTypes type, eSoundTracks track, float volume)
        {
            MMSoundManagerTrackEventTypes eventType = default;
            MMSoundManager.MMSoundManagerTracks trackType = default;
            
            switch (type)
            {
                case eSoundTrackEventTypes.MuteTrack:
                    eventType = MMSoundManagerTrackEventTypes.MuteTrack;
                    break;
                case eSoundTrackEventTypes.UnmuteTrack:
                    eventType = MMSoundManagerTrackEventTypes.UnmuteTrack;
                    break;
                case eSoundTrackEventTypes.SetVolumeTrack:
                    eventType = MMSoundManagerTrackEventTypes.SetVolumeTrack;
                    break;
                case eSoundTrackEventTypes.PlayTrack:
                    eventType = MMSoundManagerTrackEventTypes.PlayTrack;
                    break;
                case eSoundTrackEventTypes.PauseTrack:
                    eventType = MMSoundManagerTrackEventTypes.PauseTrack;
                    break;
                case eSoundTrackEventTypes.StopTrack:
                    eventType = MMSoundManagerTrackEventTypes.StopTrack;
                    break;
                case eSoundTrackEventTypes.FreeTrack:
                    eventType = MMSoundManagerTrackEventTypes.FreeTrack;
                    break;
            }
            
            switch (track)
            {
                case eSoundTracks.Master:
                    trackType = MMSoundManager.MMSoundManagerTracks.Master;
                    break;
                case eSoundTracks.Music:
                    trackType = MMSoundManager.MMSoundManagerTracks.Music;
                    break;
                case eSoundTracks.Sfx:
                    trackType = MMSoundManager.MMSoundManagerTracks.Sfx;
                    break;
                case eSoundTracks.UI:
                    trackType = MMSoundManager.MMSoundManagerTracks.UI;
                    break;
                case eSoundTracks.Other:
                    trackType = MMSoundManager.MMSoundManagerTracks.Other;
                    break;
            }
            
            MMSoundManagerTrackEvent.Trigger(eventType, trackType, volume);
        }

        public void ControlAllTrack(eAllSoundControlEventTypes type)
        {
            MMSoundManagerAllSoundsControlEventTypes eventType = default;
            
            switch (type)
            {
                case eAllSoundControlEventTypes.Play:
                    eventType = MMSoundManagerAllSoundsControlEventTypes.Play;
                    break;
                case eAllSoundControlEventTypes.Pause:
                    eventType = MMSoundManagerAllSoundsControlEventTypes.Pause;
                    break;
                case eAllSoundControlEventTypes.Stop:
                    eventType = MMSoundManagerAllSoundsControlEventTypes.Stop;
                    break;
                case eAllSoundControlEventTypes.Free:
                    eventType = MMSoundManagerAllSoundsControlEventTypes.Free;
                    break;
                case eAllSoundControlEventTypes.FreeAllButPersistent:
                    eventType = MMSoundManagerAllSoundsControlEventTypes.FreeAllButPersistent;
                    break;
                case eAllSoundControlEventTypes.FreeAllLooping:
                    eventType = MMSoundManagerAllSoundsControlEventTypes.FreeAllLooping;
                    break;
            }
            
            MMSoundManagerAllSoundsControlEvent.Trigger(eventType);
        }
        
        public void FadeTrack(eSoundTrackFadeEventModes mode, eSoundTracks track, float fadeDuration, float finalVolume, eFadeTrackTweenType fadeTween)
        {
            MMSoundManagerTrackFadeEvent.Modes fadeMode = default;
            MMSoundManager.MMSoundManagerTracks trackType = default;
            MMTweenType tweenType  = null;
            
            switch (mode)
            {
                case eSoundTrackFadeEventModes.PlayFade:
                    fadeMode = MMSoundManagerTrackFadeEvent.Modes.PlayFade;
                    break;
                case eSoundTrackFadeEventModes.StopFade:
                    fadeMode = MMSoundManagerTrackFadeEvent.Modes.StopFade;
                    break;
            }

            switch (track)
            {
                case eSoundTracks.Music:
                    trackType = MMSoundManager.MMSoundManagerTracks.Music;
                    break;
                case eSoundTracks.Sfx:
                    trackType = MMSoundManager.MMSoundManagerTracks.Sfx;
                    break;
                case eSoundTracks.UI:
                    trackType = MMSoundManager.MMSoundManagerTracks.UI;
                    break;
                case eSoundTracks.Other:
                    trackType = MMSoundManager.MMSoundManagerTracks.Other;
                    break;
            }

            switch (fadeTween)
            {
                case eFadeTrackTweenType.LinearTween:
                    tweenType = new MMTweenType(MMTween.MMTweenCurve.LinearTween);
                    break;
                case eFadeTrackTweenType.EaseInQuadratic:
                    tweenType = new MMTweenType(MMTween.MMTweenCurve.EaseInQuadratic);
                    break;
                case eFadeTrackTweenType.EaseOutQuadratic:
                    tweenType = new MMTweenType(MMTween.MMTweenCurve.EaseOutQuadratic);
                    break;
                case eFadeTrackTweenType.EaseInOutQuadratic:
                    tweenType = new MMTweenType(MMTween.MMTweenCurve.EaseInOutQuadratic);
                    break;
                case eFadeTrackTweenType.EaseInCubic:
                    tweenType = new MMTweenType(MMTween.MMTweenCurve.EaseInCubic);
                    break;
                case eFadeTrackTweenType.EaseOutCubic:
                    tweenType = new MMTweenType(MMTween.MMTweenCurve.EaseOutCubic);
                    break;
                case eFadeTrackTweenType.EaseInOutCubic:
                    tweenType = new MMTweenType(MMTween.MMTweenCurve.EaseInOutCubic);
                    break;
                case eFadeTrackTweenType.EaseInQuartic:
                    tweenType = new MMTweenType(MMTween.MMTweenCurve.EaseInQuartic);
                    break;
                case eFadeTrackTweenType.EaseOutQuartic:
                    tweenType = new MMTweenType(MMTween.MMTweenCurve.EaseOutQuartic);
                    break;
                case eFadeTrackTweenType.EaseInOutQuartic:
                    tweenType = new MMTweenType(MMTween.MMTweenCurve.EaseInOutQuartic);
                    break;
                case eFadeTrackTweenType.EaseInQuintic:
                    tweenType = new MMTweenType(MMTween.MMTweenCurve.EaseInQuintic);
                    break;
                case eFadeTrackTweenType.EaseOutQuintic:
                    tweenType = new MMTweenType(MMTween.MMTweenCurve.EaseOutQuintic);
                    break;
                case eFadeTrackTweenType.EaseInOutQuintic:
                    tweenType = new MMTweenType(MMTween.MMTweenCurve.EaseInOutQuintic);
                    break;
                case eFadeTrackTweenType.EaseInSinusoidal:
                    tweenType = new MMTweenType(MMTween.MMTweenCurve.EaseInSinusoidal);
                    break;
                case eFadeTrackTweenType.EaseOutSinusoidal:
                    tweenType = new MMTweenType(MMTween.MMTweenCurve.EaseOutSinusoidal);
                    break;
                case eFadeTrackTweenType.EaseInOutSinusoidal:
                    tweenType = new MMTweenType(MMTween.MMTweenCurve.EaseInOutSinusoidal);
                    break;
                case eFadeTrackTweenType.EaseInBounce:
                    tweenType = new MMTweenType(MMTween.MMTweenCurve.EaseInBounce);
                    break;
                case eFadeTrackTweenType.EaseOutBounce:
                    tweenType = new MMTweenType(MMTween.MMTweenCurve.EaseOutBounce);
                    break;
                case eFadeTrackTweenType.EaseInOutBounce:
                    tweenType = new MMTweenType(MMTween.MMTweenCurve.EaseInOutBounce);
                    break;
                case eFadeTrackTweenType.EaseInOverhead:
                    tweenType = new MMTweenType(MMTween.MMTweenCurve.EaseInOverhead);
                    break;
                case eFadeTrackTweenType.EaseOutOverhead:
                    tweenType = new MMTweenType(MMTween.MMTweenCurve.EaseOutOverhead);
                    break;
                case eFadeTrackTweenType.EaseInOutOverhead:
                    tweenType = new MMTweenType(MMTween.MMTweenCurve.EaseInOutOverhead);
                    break;
                case eFadeTrackTweenType.EaseInExponential:
                    tweenType = new MMTweenType(MMTween.MMTweenCurve.EaseInExponential);
                    break;
                case eFadeTrackTweenType.EaseOutExponential:
                    tweenType = new MMTweenType(MMTween.MMTweenCurve.EaseOutExponential);
                    break;
                case eFadeTrackTweenType.EaseInOutExponential:
                    tweenType = new MMTweenType(MMTween.MMTweenCurve.EaseInOutExponential);
                    break;
                case eFadeTrackTweenType.EaseInElastic:
                    tweenType = new MMTweenType(MMTween.MMTweenCurve.EaseInElastic);
                    break;
                case eFadeTrackTweenType.EaseOutElastic:
                    tweenType = new MMTweenType(MMTween.MMTweenCurve.EaseOutElastic);
                    break;
                case eFadeTrackTweenType.EaseInOutElastic:
                    tweenType = new MMTweenType(MMTween.MMTweenCurve.EaseInOutElastic);
                    break;
                case eFadeTrackTweenType.EaseInCircular:
                    tweenType = new MMTweenType(MMTween.MMTweenCurve.EaseInCircular);
                    break;
                case eFadeTrackTweenType.EaseOutCircular:
                    tweenType = new MMTweenType(MMTween.MMTweenCurve.EaseOutCircular);
                    break;
                case eFadeTrackTweenType.EaseInOutCircular:
                    tweenType = new MMTweenType(MMTween.MMTweenCurve.EaseInOutCircular);
                    break;
                case eFadeTrackTweenType.AntiLinearTween:
                    tweenType = new MMTweenType(MMTween.MMTweenCurve.AntiLinearTween);
                    break;
                case eFadeTrackTweenType.AlmostIdentity:
                    tweenType = new MMTweenType(MMTween.MMTweenCurve.AlmostIdentity);
                    break;
            }
            
            MMSoundManagerTrackFadeEvent.Trigger(fadeMode, trackType, fadeDuration, finalVolume, tweenType);
        }
    }
}