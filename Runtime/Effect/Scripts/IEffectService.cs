using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IEffectService
{
    ParticleSystem Play(string effectName, Vector3 position, Quaternion rotation,Transform attachToTransform = null);
}
