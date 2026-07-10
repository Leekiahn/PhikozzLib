using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    private Action<AudioPlayer> _onPlaybackCompleted;

    public bool IsPlaying => _audioSource != null && _audioSource.isPlaying;

    private void Awake()
    {
        if (_audioSource == null)
        {
            _audioSource = GetComponent<AudioSource>();
        }
    }

    public void Init(Action<AudioPlayer> onPlaybackCompleted)
    {
        _onPlaybackCompleted = onPlaybackCompleted;
    }

    public void Play(AudioClip clip, float volume, float pitch, bool loop)
    {
        gameObject.SetActive(true);

        _audioSource.clip = clip;
        _audioSource.volume = Mathf.Clamp01(volume);
        _audioSource.pitch = pitch;
        _audioSource.loop = loop;
        _audioSource.Play();

        if (!loop)
        {
            WaitForPlaybackEnd(clip).Forget();
        }
    }

    public void Stop()
    {
        if (_audioSource == null)
        {
            return;
        }

        _audioSource.Stop();
        _audioSource.clip = null;
    }

    private async UniTaskVoid WaitForPlaybackEnd(AudioClip clip)
    {
        await UniTask.WaitUntil(() =>
            _audioSource == null ||
            !_audioSource.isPlaying ||
            _audioSource.clip != clip ||
            !gameObject.activeInHierarchy);

        if (_audioSource == null || _audioSource.loop || _audioSource.clip != clip)
        {
            return;
        }

        Stop();
        _onPlaybackCompleted?.Invoke(this);
    }
}