using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace PhikozzLib
{
    [CreateAssetMenu(menuName = "PhikozzLib/Audio/SoundDatabase")]
    public class SoundDatabase : SerializedScriptableObject
    {
        [OdinSerialize] 
        [Searchable] 
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
        private Dictionary<string, MMF_MMSoundManagerSoundData> _sfxSoundDataDic = new();
        
        [OdinSerialize] 
        [Searchable] 
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
        private Dictionary<string, MMF_MMSoundManagerSoundData> _uiSoundDataDic = new();
        
        [OdinSerialize] 
        [Searchable] 
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
        private Dictionary<string, MMF_MMSoundManagerSoundData> _otherSoundDataDic = new();

        public Dictionary<string, MMF_MMSoundManagerSoundData> SfxSoundDataDic => _sfxSoundDataDic;
        public Dictionary<string, MMF_MMSoundManagerSoundData> UiSoundDataDic => _uiSoundDataDic;
        public Dictionary<string, MMF_MMSoundManagerSoundData> OtherSoundDataDic => _otherSoundDataDic;
    }
}