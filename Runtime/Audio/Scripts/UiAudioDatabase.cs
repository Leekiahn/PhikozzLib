using UnityEngine;
using System.Collections.Generic;
using PhikozzLib;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Audio/UI Database")]
public class UiAudioDatabase : ScriptableObject
{
    [TableList(AlwaysExpanded = true)]
    [SerializeField] private List<AudioEntry> _entries;
    
    public List<AudioEntry> Entries => _entries;
}
