using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "AudioConfig", menuName = "PhikozzLib/AudioConfig")]
public class AudioConfig : ScriptableObject
{
    [SerializeField] private AssetLabelReference _labelReference;
    [SerializeField] private AssetReference _audioDatabaseReference;
    [SerializeField] private AssetReference _playlistDatabaseReference;
    
    public AssetLabelReference LabelReference => _labelReference;
    public AssetReference AudioDatabaseReference => _audioDatabaseReference;
    public AssetReference PlaylistDatabaseReference => _playlistDatabaseReference;
}
