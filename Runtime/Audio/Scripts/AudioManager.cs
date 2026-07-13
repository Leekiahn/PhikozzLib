using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;

namespace PhikozzLib
{
    public class AudioManager : MonoBehaviour, IAudioService, IServiceRegister
    {
        [SerializeField] private AudioDatabase _audioDatabase;
        [SerializeField] private AudioPlayer _audioPlayer;

        private readonly Dictionary<string, ObjectPool<AudioPlayer>> _audioPlayerPoolDictionary = new();
        private AudioPlayer _bgmPlayer;
        
        public void RegisterService()
        {
            ServiceLocator.Register<IAudioService>(this);
        }

        #region BGM

        public void PlayBgm(string id)
        {
            if (_bgmPlayer == null)
            {
                _bgmPlayer = Instantiate(_audioPlayer, transform);
                _bgmPlayer.name = "AudioPlayer_BGM";
            }

            if (_audioDatabase.AudioDataDictionary.TryGetValue(eAudioType.BGM, out var audioDataList))
            {
                var audioData = audioDataList.Find(data => data.ID == id);
                _bgmPlayer.name = $"AudioPlayer_BGM_{audioData.ID}";
                _bgmPlayer.SetData(audioData);
                _bgmPlayer.Play();
            }
        }

        public void StopBgm()
        {
            _bgmPlayer?.Stop();
        }

        public void PauseBgm()
        {
            _bgmPlayer?.Pause();
        }

        public void ResumeBgm()
        {
            _bgmPlayer?.Resume();
        }


        #endregion
        
        public void Play(eAudioType type, string id)
        {
            if (_audioDatabase.AudioDataDictionary.TryGetValue(type, out var audioDataList))
            {
                var audioData = audioDataList.Find(data => data.ID == id);
                var pool = GetOrCreatePool(audioData);
                var player = pool.Get();
                player.name = $"AudioPlayer_{audioData.ID}";
                player.SetData(audioData);
                player.Play(() =>
                {
                    pool.Release(player);
                });
            }
        }
        
        public void Play(eAudioType type, string id, Vector3 position)
        {
            if (_audioDatabase.AudioDataDictionary.TryGetValue(type, out var audioDataList))
            {
                var audioData = audioDataList.Find(data => data.ID == id);
                var pool = GetOrCreatePool(audioData);
                var player = pool.Get();
                player.name = $"AudioPlayer_{audioData.ID}";
                player.transform.position = position;
                player.SetData(audioData);
                player.Play(() =>
                {
                    pool.Release(player);
                });
            }
        }

        public void PlayRandom(eAudioType type, string id)
        {
        }

        public void Stop(string id)
        {
            if (_audioPlayerPoolDictionary.TryGetValue(id, out var pool))
            {
                var player = pool.Get();
                player.Stop();
            }
        }

        public void Pause(string id)
        {
            if (_audioPlayerPoolDictionary.TryGetValue(id, out var pool))
            {
                var player = pool.Get();
                player.Pause();
            }
        }

        public void Resume(string id)
        {
            if (_audioPlayerPoolDictionary.TryGetValue(id, out var pool))
            {
                var player = pool.Get();
                player.Resume();
            }
        }
        
        private ObjectPool<AudioPlayer> GetOrCreatePool(AudioData audioData)
        {
            if (_audioPlayerPoolDictionary.TryGetValue(audioData.ID, out var pool))
            {
                return pool;
            }

            pool = new ObjectPool<AudioPlayer>(() =>
            {
                var player = Instantiate(_audioPlayer, transform);
                player.SetData(audioData);
                return player;
            }, player =>
            {
                player.gameObject.SetActive(true);
            }, player =>
            {
                player.gameObject.SetActive(false);
            }, player =>
            {
                Destroy(player.gameObject);
            }, false, 8, 16);

            _audioPlayerPoolDictionary[audioData.ID] = pool;
            return pool;
        }
    }
}