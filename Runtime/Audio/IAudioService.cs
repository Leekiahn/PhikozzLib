using UnityEngine;

namespace PhikozzLib
{
    public interface IAudioService
    {
        void PlaySfx(AudioClip clip);
        void PlayBgm(AudioClip clip, bool loop = true);
        void PauseBgm();
        void ResumeBgm();
        void StopBgm();
    
        float GetMasterVolume();
        float GetSfxVolume();
        float GetBgmVolume();
        void SetMasterVolume(float volume);
        void SetSfxVolume(float volume);
        void SetBgmVolume(float volume);
    }
}
