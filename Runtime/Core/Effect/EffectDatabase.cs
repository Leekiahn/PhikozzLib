using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace PhikozzLib
{
    [CreateAssetMenu(fileName = "EffectDatabase", menuName = "PhikozzLib/EffectDatabase", order = 60)]
    public class EffectDatabase : SerializedScriptableObject
    {
        [OdinSerialize] 
        [Searchable] 
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
        private Dictionary<string, ParticleSystem> _particleSystemDic = new();
    
        public Dictionary<string, ParticleSystem> ParticleSystemDic => _particleSystemDic;
    }
}

