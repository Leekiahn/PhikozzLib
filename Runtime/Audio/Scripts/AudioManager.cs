using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace PhikozzLib
{
    public class AudioManager : MonoBehaviour, IAudioService, IServiceRegister
    {
        [SerializeField] private AudioDatabase _audioDatabase;
        [SerializeField] private AudioPlayer _audioPlayer;
        
        private TrackedPool<AudioPlayer> _pool;
        
        public void RegisterService()
        {
            ServiceLocator.Register<IAudioService>(this);
        }

        public void Play(eAudioDatabaseType type, string id)
        {
            
        }

        public void Stop()
        {
            
        }

        public void Pause()
        {
            
        }

        public void Resume()
        {
            
        }

        public List<AudioData> GetAudioDataListById(eAudioDatabaseType type, string id)
        {
            if (_audioDatabase.AudioDataDictionary.TryGetValue(type, out var audioDataList))
            {
                return audioDataList.FindAll(audioData => audioData.ID == id);
            }
            
            return null;
        }
        
        public AudioData GetAudioDataById(eAudioDatabaseType type, string id)
        {
            if (_audioDatabase.AudioDataDictionary.TryGetValue(type, out var audioDataList))
            {
                return audioDataList.Find(audioData => audioData.ID == id);
            }
            
            return null;
        }
    }
}