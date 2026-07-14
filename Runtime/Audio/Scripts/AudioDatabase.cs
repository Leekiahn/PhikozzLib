using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace PhikozzLib
{
    [CreateAssetMenu(menuName = "Audio/AudioDatabase")]
    public class AudioDatabase : SerializedScriptableObject
    {
        [OdinSerialize] 
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
        private Dictionary<eAudioType, List<AudioData>> _audioDataDictionary = new()
        {
            { eAudioType.BGM, new List<AudioData>() },
            { eAudioType.SFX, new List<AudioData>() },
            { eAudioType.UI, new List<AudioData>() }
        };

        public Dictionary<eAudioType, List<AudioData>> AudioDataDictionary => _audioDataDictionary;
    }
}