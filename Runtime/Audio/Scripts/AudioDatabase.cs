using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using PhikozzLib;

[CreateAssetMenu(menuName = "Audio/AudioDatabase")]
public class AudioDatabase : SerializedScriptableObject
{
    [OdinSerialize]
    [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
    private Dictionary<eAudioDatabaseType, List<AudioData>> _audioDataDictionary = new()
    {
        {eAudioDatabaseType.BGM, new List<AudioData>()},
        {eAudioDatabaseType.SFX, new List<AudioData>()},
        {eAudioDatabaseType.UI, new List<AudioData>()}
    };
    
    public Dictionary<eAudioDatabaseType, List<AudioData>> AudioDataDictionary => _audioDataDictionary;
}