using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "AudioData", menuName = "PhikozzLib/AudioData", order = 0)]
public class AudioData : ScriptableObject
{
    [BoxGroup("Audio Clip")]
    [SerializeField] private AudioClip _audioClip;
    
    [BoxGroup("Settings")]
    [SerializeField] private AudioMixerGroup _audioMixerGroup;
    // [BoxGroup("Settings")]
    // [SerializeField] private float _volume = 1f;
    // [BoxGroup("Settings")]
    // [SerializeField] private float _pitch = 1f;
    // [BoxGroup("Settings")]
    // [SerializeField] private bool _loop = false;
}
