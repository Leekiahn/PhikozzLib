using UnityEngine;

public interface IAudioSettings 
{
    float GetMasterVolume();
    float GetSfxVolume();
    float GetBgmVolume();
    
    void SetMasterVolume(float volume);
    void SetSfxVolume(float volume);
    void SetBgmVolume(float volume);
}
