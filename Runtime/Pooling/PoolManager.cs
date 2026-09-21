using PhikozzLib;
using UnityEngine;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour, IPoolService, IServiceRegister
{
    private class Pool<T> : IPool where T : Component
    {
        private readonly TrackedPool<T> _pool;

        public Pool(T prefab, Transform parent, int defaultCapacity = 10, int maxSize = 20)
        {
            _pool = new TrackedPool<T>(
                onCreate: () =>
                {
                    var instance = Instantiate(prefab, parent);
                    instance.gameObject.SetActive(false);
                    return instance;
                },
                onGet: (instance) =>
                {
                    instance.gameObject.SetActive(true);
                },
                onRelease: (instance) =>
                {
                    instance.gameObject.SetActive(false);
                },
                onDestroy: (instance) =>
                {
                    Destroy(instance.gameObject);
                },
                defaultCapacity: defaultCapacity,
                maxSize: maxSize
            );
        }

        public Component Get()
        {
            return _pool.Get();
        }

        public void Release(Component instance)
        {
            _pool.Release((T)instance);
        }

        public void ReleaseAll()
        {
            _pool.ReleaseAll();
        }

        public void Clear()
        {
            _pool.Clear();
        }
    }

    public void RegisterService()
    {
        ServiceLocator.Register<IPoolService>(this);
    }

    public void UnregisterService()
    {
        ServiceLocator.Unregister<IPoolService>();
    }

    private readonly Dictionary<string, IPool> _pools = new();

    public void RegisterPool<T>(string key, T prefab, Transform parent, int defaultCapacity = 10, int maxSize = 20) where T : Component
    {
        if (!_pools.ContainsKey(key))
        {
            var pool = new Pool<T>(prefab, parent, defaultCapacity, maxSize);
            _pools[key] = pool;
        }
    }

    public void UnregisterPool(string key)
    {
        if (_pools.ContainsKey(key))
        {
            _pools[key].ReleaseAll();
            _pools[key].Clear();
            _pools.Remove(key);
        }
    }

    public T Spawn<T>(string key, Vector3 position, Quaternion rotation) where T : Component
    {
        if (_pools.ContainsKey(key))
        {
            var instance = (T)_pools[key].Get();
            instance.transform.SetPositionAndRotation(position, rotation);
            return instance;
        }

        return null;
    }

    public void Despawn<T>(string key, T instance) where T : Component
    {
        if (_pools.ContainsKey(key))
        {
            _pools[key].Release(instance);
        }
    }

    public void DespawnAll(string key)
    {
        if (_pools.ContainsKey(key))
        {
            _pools[key].ReleaseAll();
        }
    }

    public void ClearPool(string key)
    {
        if (_pools.ContainsKey(key))
        {
            _pools[key].Clear();
        }
    }

    public void ClearAllPools()
    {
        foreach (var pool in _pools.Values)
        {
            pool.Clear();
        }
    }
}
