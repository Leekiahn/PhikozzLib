using UnityEngine;

namespace PhikozzLib
{
    public interface IAudioService
    {
        void Play(eAudioType type, string id);
    }
}