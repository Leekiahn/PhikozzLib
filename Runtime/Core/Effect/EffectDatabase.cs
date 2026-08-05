using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PhikozzLib
{
    [CreateAssetMenu(fileName = "EffectDatabase", menuName = "PhikozzLib/EffectDatabase", order = 60)]
    public class EffectDatabase : ScriptableObject
    {
        [Serializable]
        public class ParticleSystemData
        {
            [LabelText("Particle Key")]
            [SerializeField] private string _particleKey;

            [LabelText("Prefab")]
            [SerializeField] private ParticleSystem _particleSystem;

            public string ParticleKey => _particleKey;
            public ParticleSystem ParticleSystem => _particleSystem;
        }

        [Serializable]
        public class EffectGroup
        {
            [TitleGroup("$Key")]
            [HideLabel]
            [SerializeField] private string _key;

            [TitleGroup("$Key")]
            [ListDrawerSettings]
            [Searchable]
            [SerializeField] private List<ParticleSystemData> _particles = new();

            public string Key => _key;
            public List<ParticleSystemData> Particles => _particles;
        }

        [ListDrawerSettings]
        [SerializeField] private List<EffectGroup> _effectGroups = new();

        public List<EffectGroup> EffectGroups => _effectGroups;
    }
}