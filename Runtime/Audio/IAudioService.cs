using UnityEngine;

public interface IAudioService
{
    void PlaySfx(AudioClip clip);
    void PlayBgm(AudioClip clip, bool loop = true);
    void PauseBgm();
    void ResumeBgm();
    void StopBgm();
}
