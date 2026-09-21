using UnityEngine;

public interface IPoolService
{
    void RegisterPool<T>(string key, T prefab, Transform parent, int defaultCapacity = 10, int maxSize = 20) where T : Component;
    void UnregisterPool(string key);
    T Spawn<T>(string key, Vector3 position, Quaternion rotation) where T : Component;
    void Despawn<T>(string key, T instance) where T : Component;
}
