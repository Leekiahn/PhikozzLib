using System;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace PhikozzLib
{
    [CreateAssetMenu(fileName = "AudioDatabase", menuName = "PhikozzLib/AudioDatabase", order = 50)]
    public class AudioDatabase : SerializedScriptableObject
    {
        [Serializable]
        public class SoundEntry
        {
            [LabelText("Audio Key")]
            [SerializeField] private string _audioKey;

            [LabelText("Audio Clip")]
            [SerializeField] private MMF_MMSoundManagerSoundData _soundData;

            public string AudioKey => _audioKey;
            public MMF_MMSoundManagerSoundData SoundData => _soundData;
        }
        
        [Serializable]
        public class SoundGroup
        {
            [TitleGroup("$Key")]
            [HideLabel]
            [SerializeField] private string _key;

            [TitleGroup("$Key")]
            [ListDrawerSettings]
            [Searchable]
            [SerializeField] private List<SoundEntry> _soundEntries = new();

            public string Key => _key;
            public List<SoundEntry> SoundEntries => _soundEntries;
        }
        
        [ListDrawerSettings]
        [SerializeField] private List<SoundGroup> _soundGroups = new();

        public List<SoundGroup> SoundGroups => _soundGroups;
    }
}