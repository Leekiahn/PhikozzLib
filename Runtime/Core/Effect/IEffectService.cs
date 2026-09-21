using UnityEngine;

namespace PhikozzLib
{
    public interface IEffectService
    {
        void RegisterEffect(string key, ParticleSystem prefab);
        void UnregisterEffect(string key);
        ParticleSystem Play(string key, Vector3 position, Quaternion rotation, float duration = 0f, Transform attachToTransform = null);
    }
}

