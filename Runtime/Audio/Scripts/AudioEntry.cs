using System;
using UnityEngine;
using UnityEngine.AddressableAssets;


[Serializable]
public class AudioEntry
{
    [SerializeField] private string _id;
    [SerializeField] private AssetReferenceT<AudioClip> _clip;
    [SerializeField] private float _volume = 1f;
    [SerializeField] private float _pitch = 1f;
    [SerializeField] private bool _loop;
    
    public string ID => _id;
    public AssetReferenceT<AudioClip> Clip => _clip;
    public float Volume => _volume;
    public float Pitch => _pitch;
    public bool Loop => _loop;
}