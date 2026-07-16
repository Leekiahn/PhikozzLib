using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "PhikozzLib/Data", order = 0)]
public class Data : SerializedScriptableObject
{
    [OdinSerialize] 
    [Searchable] 
    [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
    private Dictionary<string, GameObject> gameObjects = new Dictionary<string, GameObject>();
    
    public Dictionary<string, GameObject> GameObjects => gameObjects;
}
