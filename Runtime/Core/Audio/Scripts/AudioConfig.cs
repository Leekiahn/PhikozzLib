using PhikozzLib;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioConfig", menuName = "PhikozzLib/Audio/AudioConfig")]
public class AudioConfig : ScriptableObject
{
    [SerializeField] private PlaylistDatabase _playlistDatabase;
    [SerializeField] private SoundDatabase _soundDatabase;

    public PlaylistDatabase PlaylistDatabase => _playlistDatabase;
    public SoundDatabase SoundDatabase => _soundDatabase;
}