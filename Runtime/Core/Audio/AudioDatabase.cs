using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace PhikozzLib
{
    [CreateAssetMenu(fileName = "AudioDatabase", menuName = "PhikozzLib/AudioDatabase", order = 40)]
    public class AudioDatabase : SerializedScriptableObject
    {
        [OdinSerialize] 
        [Searchable] 
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
        private Dictionary<string, MMF_MMSoundManagerSoundData> _sfxAudioDataDic = new();
        
        [OdinSerialize] 
        [Searchable] 
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
        private Dictionary<string, MMF_MMSoundManagerSoundData> _uiAudioDataDic = new();
        
        [OdinSerialize] 
        [Searchable] 
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
        private Dictionary<string, MMF_MMSoundManagerSoundData> _otherAudioDataDic = new();

        public Dictionary<string, MMF_MMSoundManagerSoundData> SfxAudioDataDic => _sfxAudioDataDic;
        public Dictionary<string, MMF_MMSoundManagerSoundData> UIAudioDataDic => _uiAudioDataDic;
        public Dictionary<string, MMF_MMSoundManagerSoundData> OtherAudioDataDic => _otherAudioDataDic;
    }
}