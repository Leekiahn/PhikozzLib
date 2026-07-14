using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace PhikozzLib
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        
        public void SetData(AudioData audioData)
        {
            _audioSource.clip = audioData.Clip;
            _audioSource.outputAudioMixerGroup = audioData.MixerGroup;
            _audioSource.volume = audioData.Volume;
            _audioSource.pitch = audioData.Pitch;
            _audioSource.loop = audioData.Loop;
        }
        
        public void Play(Action onComplete = null)
        {
            _audioSource.Play();
            WaitForComplete(onComplete).Forget();
        }

        public void PlayRandom(Action onComplete = null)
        {
            // RandomAudioData를 구현해야 할 듯
        }
        
        public void Stop()
        {
            _audioSource.Stop();
        }

        public void Pause()
        {
            _audioSource.Pause();
        }

        public void Resume()
        {
            _audioSource.UnPause();
        }
        
        private async UniTaskVoid WaitForComplete(Action onComplete)
        {
            await UniTask.WaitWhile(() => _audioSource.isPlaying, cancellationToken: destroyCancellationToken);
            onComplete?.Invoke();
        }
    }
}