using PhikozzLib;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioConfig", menuName = "PhikozzLib/Audio/AudioConfig")]
public class AudioConfig : ScriptableObject
{
    [SerializeField] private PlaylistDatabase _playlistDatabase;
    [SerializeField] private AudioDatabase audioDatabase;

    public PlaylistDatabase PlaylistDatabase => _playlistDatabase;
    public AudioDatabase AudioDatabase => audioDatabase;
}