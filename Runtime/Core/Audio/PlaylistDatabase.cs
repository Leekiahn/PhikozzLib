using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

namespace PhikozzLib
{
    [CreateAssetMenu(fileName = "PlaylistDatabase", menuName = "PhikozzLib/PlaylistDatabase", order = 30)]
    public class PlaylistDatabase : SerializedScriptableObject
    {
        [OdinSerialize]
        [Searchable]
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
        private Dictionary<string, MMSMPlaylist> _playlistDic = new();
    
        public Dictionary<string, MMSMPlaylist> PlaylistDic => _playlistDic;
    }
}

