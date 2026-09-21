using UnityEngine;

namespace PhikozzLib
{
    public interface IAudioService
    {
        void RegisterAudio(string key, AudioClip clip);
        void UnregisterAudio(string key);
        AudioSource PlayBgm(string key);
        void StopBgm();
        AudioSource PlaySfx(string key, Vector3 position, float spatialBlend = 0f, float volume = 1f, float pitch = 1f);
        void SetMasterVolume(float volume);
        void SetBgmVolume(float volume);
        void SetSfxVolume(float volume);
    }
}
