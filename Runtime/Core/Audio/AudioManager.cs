using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using MoreMountains.Tools;
using Sirenix.OdinInspector;

namespace PhikozzLib
{
    public class AudioManager : MonoBehaviour, IAudioService, IServiceRegister
    {
        [InfoBox("AudioManager 프리팹 하위에 MMSMPlaylistManager, MMSoundManager를 배치해야 합니다.")] 

        [SerializeField] private PlaylistDatabase _playlistDatabase;
        [SerializeField] private AudioDatabase _audioDatabase;
        
        private MMSMPlaylistManager _playlistManager;
        private readonly Dictionary<string, Dictionary<string, MMF_MMSoundManagerSoundData>> _soundGroupDic = new();

        private void Awake()
        {
            _playlistManager = GetComponentInChildren<MMSMPlaylistManager>();
            
            foreach (var soundGroup in _audioDatabase.SoundGroups)
            {
                var soundDic = new Dictionary<string, MMF_MMSoundManagerSoundData>();
                foreach (var soundEntry in soundGroup.SoundEntries)
                {
                    soundDic[soundEntry.AudioKey] = soundEntry.SoundData;
                }
                _soundGroupDic[soundGroup.Key] = soundDic;
            }
        }
        
        public void RegisterService()
        {
            ServiceLocator.Register<IAudioService>(this);
        }

        #region --------------- BGM ---------------
        
        [PropertySpace(SpaceBefore = 30f)]
        [Title("BGM")]
        [Button(ButtonSizes.Medium, ButtonStyle.Box)]
        public void PlayBgm(string channelKey, int index)
        {
            if (_playlistDatabase.PlaylistDic.TryGetValue(channelKey, out var playlist))
            {
                if (_playlistManager.Playlist == playlist)
                {
                    _playlistManager.PlaySongAt(index);
                    return;
                }

                _playlistManager.ChangePlaylist(playlist);
            }
        }

        [ButtonGroup]
        public void StopBgm()
        {
            _playlistManager.Stop();
        }

        [ButtonGroup]
        public void PauseBgm()
        {
            _playlistManager.Pause();
        }

        [ButtonGroup]
        public void ResumeBgm()
        {
            _playlistManager.Play();
        }

        [ButtonGroup]
        public void PlayNextBgm()
        {
            _playlistManager.PlayNextSong();
        }

        [ButtonGroup]
        public void PlayPreviousBgm()
        {
            _playlistManager.PlayPreviousSong();
        }

        [Button(ButtonSizes.Medium, ButtonStyle.Box)]
        public void SetBgmMultiplier(float volume = 1f, float pitch = 1f, bool instantly = true)
        {
            _playlistManager.SetVolumeMultiplier(volume);
            _playlistManager.SetPitchMultiplier(pitch);
        }

        #endregion
        

        #region -------------- SFX/UI --------------

        [PropertySpace(SpaceBefore = 30f)]
        [Title("SFX/UI/Other")]
        [Button(ButtonSizes.Medium, ButtonStyle.Box)]
        public void Play(string groupName, string soundName, Vector3 position = default, Transform attachToTransform = null)
        {
            if (_soundGroupDic.TryGetValue(groupName, out var soundDic) && soundDic.TryGetValue(soundName, out var soundData))
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

        #endregion

        #region -------------- Track --------------

        [PropertySpace(SpaceBefore = 30f)]
        [Title("Tracks")]
        [Button(ButtonSizes.Medium, ButtonStyle.Box)]
        public void ControlTrack(eSoundTrackEventTypes type, eSoundTracks track, float volume = 1f)
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

        [Button(ButtonSizes.Medium, ButtonStyle.Box)]
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

        [Button(ButtonSizes.Medium, ButtonStyle.Box)]
        public void FadeTrack(eSoundTrackFadeEventModes mode, eSoundTracks track, float fadeDuration, float finalVolume,
            eFadeTrackTweenType fadeTween)
        {
            MMSoundManagerTrackFadeEvent.Modes fadeMode = default;
            MMSoundManager.MMSoundManagerTracks trackType = default;
            MMTweenType tweenType = null;

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

        #endregion
    }
}