using UnityEngine;
using System.Collections.Generic;

namespace PhikozzLib
{
    public interface IAudioService
    {
        public List<AudioData> GetDataListById(eAudioDatabaseType type, string id);
        public AudioData GetDataById(eAudioDatabaseType type, string id);
    }
}
