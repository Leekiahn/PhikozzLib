using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Audio/AudioDatabase")]
public class AudioDatabase : ScriptableObject
{
    [Title("BGM")]
    [TableList(AlwaysExpanded = false, DrawScrollView = true, MinScrollViewHeight = 120, MaxScrollViewHeight = 220)]
    [SerializeField] private List<AudioData> _bgmDataList;

    [Title("SFX")]
    [TableList(AlwaysExpanded = false, DrawScrollView = true, MinScrollViewHeight = 120, MaxScrollViewHeight = 220)]
    [SerializeField] private List<AudioData> _sfxDataList;

    [Title("UI")]
    [TableList(AlwaysExpanded = false, DrawScrollView = true, MinScrollViewHeight = 120, MaxScrollViewHeight = 220)]
    [SerializeField] private List<AudioData> _uiDataList;

    public List<AudioData> BgmDataList => _bgmDataList;
    public List<AudioData> SfxDataList => _sfxDataList;
    public List<AudioData> UiDataList => _uiDataList;
}