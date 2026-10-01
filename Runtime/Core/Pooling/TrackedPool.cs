using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace PhikozzLib
{
    public class TrackedPool<T> where T : Component
    {
        private readonly ObjectPool<T> _pool;
        private readonly Stack<T> _activeObjects = new();
        private readonly HashSet<T> _activeLookup = new();

        public TrackedPool(
            Func<T> onCreate,
            Action<T> onGet = null,
            Action<T> onRelease = null,
            Action<T> onDestroy = null,
            int defaultCapacity = 100,
            int maxSize = 20)
        {
            _pool = new ObjectPool<T>(
                createFunc: onCreate,
                actionOnGet: obj => onGet?.Invoke(obj),
                actionOnRelease: obj => onRelease?.Invoke(obj),
                actionOnDestroy: obj => onDestroy?.Invoke(obj),
                collectionCheck: true,
                defaultCapacity: defaultCapacity,
                maxSize: maxSize
            );
        }

        public T Get()
        {
            var obj = _pool.Get();
            _activeObjects.Push(obj);
            _activeLookup.Add(obj);
            return obj;
        }

        public void Release()
        {
            while (_activeObjects.Count > 0)
            {
                var obj = _activeObjects.Pop();
                if (_activeLookup.Remove(obj))
                {
                    _pool.Release(obj);
                    return;
                }
            }

            throw new InvalidOperationException("No active objects to release.");
        }

        public void Release(T obj)
        {
            if (_activeLookup.Remove(obj))
            {
                _pool.Release(obj);
            }
        }

        public void ReleaseAll()
        {
            while (_activeObjects.Count > 0)
            {
                var obj = _activeObjects.Pop();
                if (_activeLookup.Remove(obj))
                {
                    _pool.Release(obj);
                }
            }
        }

        public void Clear()
        {
            ReleaseAll();
            _pool.Clear();
        }
    }
}