using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace PhikozzLib
{
    public class PoolManager : MonoBehaviour, IPoolService, IServiceRegister
    {
        private Dictionary<string, object> _pools = new();
        private Dictionary<string, HashSet<MonoBehaviour>> _activeObjects = new();

        private const int PoolDefaultCapacity = 10;
        private const int PoolMaxSize = 100;

        public void RegisterService()
        {
            ServiceLocator.Register<IPoolService>(this);
        }

        private ObjectPool<T> CreatePool<T>(T prefab, int defaultCapacity = PoolDefaultCapacity, int maxSize = PoolMaxSize)
            where T : MonoBehaviour, IPoolable
        {
            var pool = new ObjectPool<T>(
                createFunc: () =>
                {
                    var obj = Instantiate(prefab);
                    obj.name = prefab.name;
                    obj.OnCreate();
                    obj.gameObject.SetActive(false);
                    return obj;
                },
                actionOnGet: obj =>
                {
                    obj.OnGet();
                    obj.gameObject.SetActive(true);
                },
                actionOnRelease: obj =>
                {
                    obj.OnRelease();
                    obj.gameObject.SetActive(false);
                },
                actionOnDestroy: obj =>
                {
                    if (obj != null)
                    {
                        obj.OnDestroy();
                        Destroy(obj.gameObject);
                    }
                },
                collectionCheck: false,
                defaultCapacity: defaultCapacity,
                maxSize: maxSize
            );

            _pools.Add(prefab.name, pool);
            return pool;
        }

        public T Get<T>(T prefab) where T : MonoBehaviour, IPoolable
        {
            var key = prefab.name;
            var pool = GetPool<T>(key) ?? CreatePool(prefab);

            var obj = pool.Get();

            if (!_activeObjects.ContainsKey(key))
            {
                _activeObjects[key] = new HashSet<MonoBehaviour>();
            }

            _activeObjects[key].Add(obj);

            return obj;
        }

        public void Release<T>(T obj) where T : MonoBehaviour, IPoolable
        {
            var key = obj.name;

            GetPool<T>(key)?.Release(obj);

            if (_activeObjects.TryGetValue(key, out var objs))
            {
                objs.Remove(obj);
            }
        }

        public void ReleaseAll<T>(T prefab) where T : MonoBehaviour, IPoolable
        {
            var key = prefab.name;

            if (_activeObjects.TryGetValue(key, out var objs))
            {
                foreach (var obj in objs)
                {
                    GetPool<T>(key)?.Release(obj as T);
                }

                _activeObjects.Remove(key);
            }
        }

        public void DestroyAll<T>(T prefab) where T : MonoBehaviour, IPoolable
        {
            var key = prefab.name;

            var pool = GetPool<T>(key);
            if (pool != null)
            {
                while (pool.CountInactive > 0)
                {
                    Destroy(pool.Get().gameObject);
                }

                pool.Dispose();
                _pools.Remove(key);
            }

            if (_activeObjects.TryGetValue(key, out var objs))
            {
                foreach (var obj in objs)
                {
                    Destroy(obj.gameObject);
                }

                _activeObjects.Remove(key);
            }
        }

        private ObjectPool<T> GetPool<T>(string key) where T : MonoBehaviour, IPoolable
        {
            if (_pools.TryGetValue(key, out var pool))
            {
                return pool as ObjectPool<T>;
            }

            return null;
        }
    }
}