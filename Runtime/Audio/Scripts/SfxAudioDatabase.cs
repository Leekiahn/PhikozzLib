using UnityEngine;
using System.Collections.Generic;
using PhikozzLib;
using Sirenix.OdinInspector;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "Audio/SFX Database")]
public class SfxAudioDatabase : ScriptableObject
{
    [TableList(AlwaysExpanded = true)]
    [SerializeField] private List<AudioEntry> _entries;

    public List<AudioEntry> Entries => _entries;
}
