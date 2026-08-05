using UnityEngine;

namespace PhikozzLib
{
    public interface IEffectService
    {
        ParticleSystem Play(string categoryKey, string particleKey, Vector3 position, Quaternion rotation, float duration = 0f, Transform attachToTransform = null);
    }
}

