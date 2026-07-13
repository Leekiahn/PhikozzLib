using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PhikozzLib
{
    [InlineEditor]
    [CreateAssetMenu(menuName = "Audio/AudioData")]
    public class AudioData : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private AssetReferenceT<AudioClip> _clip;
        [SerializeField, Range(0f, 1f)] private float _volume = 1f;
        [SerializeField, Range(0f, 3f)] private float _pitch = 1f;
        [SerializeField] private bool _loop;

        public string ID => _id;
        public AssetReferenceT<AudioClip> Clip => _clip;
        public float Volume => _volume;
        public float Pitch => _pitch;
        public bool Loop => _loop;
    }
}