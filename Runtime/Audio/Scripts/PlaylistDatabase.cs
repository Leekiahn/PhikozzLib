using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

namespace PhikozzLib
{
    [CreateAssetMenu(menuName = "Audio/PlaylistDatabase")]
    public class PlaylistDatabase : SerializedScriptableObject
    {
        [OdinSerialize]
        [Searchable]
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
        private Dictionary<int, MMSMPlaylist> _playlistDic = new();
    
        public Dictionary<int, MMSMPlaylist> PlaylistDic => _playlistDic;
    }
}

