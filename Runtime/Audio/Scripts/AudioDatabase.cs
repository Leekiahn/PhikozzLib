using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Audio/Database")]
public class AudioDatabase : ScriptableObject
{
    [Title("BGM")]
    [TableList(AlwaysExpanded = false, DrawScrollView = true, MinScrollViewHeight = 120, MaxScrollViewHeight = 220)]
    [SerializeField] private List<AudioEntry> _bgmEntries;

    [Title("SFX")]
    [TableList(AlwaysExpanded = false, DrawScrollView = true, MinScrollViewHeight = 120, MaxScrollViewHeight = 220)]
    [SerializeField] private List<AudioEntry> _sfxEntries;

    [Title("UI")]
    [TableList(AlwaysExpanded = false, DrawScrollView = true, MinScrollViewHeight = 120, MaxScrollViewHeight = 220)]
    [SerializeField] private List<AudioEntry> _uiEntries;

    public List<AudioEntry> BGMEntries => _bgmEntries;
    public List<AudioEntry> SfxEntries => _sfxEntries;
    public List<AudioEntry> UiEntries => _uiEntries;
}