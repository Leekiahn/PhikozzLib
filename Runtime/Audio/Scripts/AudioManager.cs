using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;
using MoreMountains.Tools;

namespace PhikozzLib
{
    public class AudioManager : MonoBehaviour, IAudioService, IServiceRegister
    {
        [SerializeField] private AudioDatabase _audioDatabase;

        public void RegisterService()
        {
            ServiceLocator.Register<IAudioService>(this);
        }

        public void Play(eAudioType type, string id)
        {
            switch (type)
            {
                case eAudioType.BGM:
                    PlayBgm(id);
                    break;
                case eAudioType.SFX:
                    PlaySfx(id);
                    break;
                //case eAudioType.UI:
                    //PlayUI(id);
                    break;
                default:
                    Debug.LogWarning($"Audio type {type} is not supported.");
                    break;
            }
        }

        private void PlayBgm(string id)
        {
            if (_audioDatabase.AudioDataDictionary.TryGetValue(eAudioType.BGM, out var audioDataList))
            {
                var audioData = audioDataList.Find(data => data.ID == id);
                MMSoundManagerPlayOptions options = new MMSoundManagerPlayOptions();
                options.Volume = audioData.Volume;
                options.Pitch = audioData.Pitch;
                options.Loop = audioData.Loop;
                options.AudioGroup = audioData.MixerGroup;
                options.SoloAllTracks = true;
                
                MMSoundManagerSoundPlayEvent.Trigger(audioData.Clip, options);
            }
        }

        private void PlaySfx(string id)
        {
            if (_audioDatabase.AudioDataDictionary.TryGetValue(eAudioType.SFX, out var audioDataList))
            {
                var  audioData = audioDataList.Find(data => data.ID == id);
                MMSoundManagerPlayOptions options = new MMSoundManagerPlayOptions();
                options.Volume = audioData.Volume;
                options.Pitch = audioData.Pitch;
                options.Loop = audioData.Loop;
                options.AudioGroup = audioData.MixerGroup;
                
                MMSoundManagerSoundPlayEvent.Trigger(audioData.Clip, options);
            }
        }
    }
}