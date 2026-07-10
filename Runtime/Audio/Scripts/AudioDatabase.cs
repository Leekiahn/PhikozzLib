using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Database")]
public class AudioDatabase : ScriptableObject
{
    [SerializeField] private BgmDatabase _bgmDatabase;
    [SerializeField] private SfxAudioDatabase _sfxDatabase;
    [SerializeField] private UiAudioDatabase _uiDatabase;
    
    public BgmDatabase BgmDatabase => _bgmDatabase;
    public SfxAudioDatabase SfxDatabase => _sfxDatabase;
    public UiAudioDatabase UiDatabase => _uiDatabase;
}
