using PhikozzLib;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

public class PoolManager : MonoBehaviour, IPoolService, IServiceRegister
{
    private readonly Dictionary<Type, object> _pools = new();


    public void RegisterService()
    {
        ServiceLocator.Register<IPoolService>(this);
    }

    public void RegisterPool<T>(TrackedPool<T> pool) where T : Component
    {
        if (!_pools.ContainsKey(typeof(T)))
        {
            _pools[typeof(T)] = pool;
        }
    }

    public void UnregisterPool<T>() where T : Component
    {
        if (_pools.ContainsKey(typeof(T)))
        {
            _pools.Remove(typeof(T));
        }
    }
    
    public TrackedPool<T> GetPool<T>() where T : Component
    {
        if (_pools.TryGetValue(typeof(T), out var pool))
        {
            return pool as TrackedPool<T>;
        }
        return null;
    }
}