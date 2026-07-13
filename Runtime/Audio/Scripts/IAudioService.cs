using UnityEngine;

namespace PhikozzLib
{
    public interface IAudioService
    {
        void PlayBgm(string id);
        void Play(eAudioType audioType, string id);
        
    }
}