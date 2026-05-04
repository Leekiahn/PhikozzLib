using UnityEngine;
using UnityEngine.Audio;

public class AudioService : MonoBehaviour, IAudioService
{
    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _sfxSource;
    
    public void PlaySfx(AudioClip clip)
    {
        _sfxSource.PlayOneShot(clip);
    }

    public void PlayBgm(AudioClip clip, bool loop = true)
    {
        _bgmSource.clip = clip;
        _bgmSource.loop = loop;
        _bgmSource.Play();
    }

    public void PauseBgm()
    {
        _bgmSource.Pause();
    }

    public void ResumeBgm()
    {
        _bgmSource.UnPause();
    }

    public void StopBgm()
    {
        _bgmSource.Stop();
    }
}
