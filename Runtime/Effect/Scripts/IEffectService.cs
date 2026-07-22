using UnityEngine;

namespace PhikozzLib
{
    public interface IEffectService
    {
        ParticleSystem Play(string key, Vector3 position, Quaternion rotation,Transform attachToTransform = null);
    }
}

