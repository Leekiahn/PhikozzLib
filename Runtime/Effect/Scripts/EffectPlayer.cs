using System;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class EffectPlayer : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    
    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    public void Play(bool withChildren)
    {
        _particleSystem.Play(withChildren);
    }
    
    public void Stop(bool withChildren, ParticleSystemStopBehavior stopBehavior)
    {
        _particleSystem.Stop(withChildren, stopBehavior);
    }

    public void Clear(bool withChildren)
    {
        _particleSystem.Clear(withChildren);
    }

    public bool IsAlive(bool withChildren)
    {
        return _particleSystem.IsAlive(withChildren);
    }
}
