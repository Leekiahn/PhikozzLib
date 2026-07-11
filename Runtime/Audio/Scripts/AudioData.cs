using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "Audio/AudioData")]
public class AudioData : ScriptableObject
{
    [TableColumnWidth(140, Resizable = true)]
    [SerializeField] private string _id;

    [TableColumnWidth(140, Resizable = true)]
    [SerializeField] private AssetReferenceT<AudioClip> _clip;

    [TableColumnWidth(60, Resizable = false)]
    [SerializeField] private float _volume = 1f;

    [TableColumnWidth(60, Resizable = false)]
    [SerializeField] private float _pitch = 1f;

    [TableColumnWidth(60, Resizable = false)]
    [SerializeField] private bool _loop;

    public string ID => _id;
    public AssetReferenceT<AudioClip> Clip => _clip;
    public float Volume => _volume;
    public float Pitch => _pitch;
    public bool Loop => _loop;
}