using UnityEngine;
using UnityEngine.Pool;

public interface IPoolService
{
    public void RegisterPool<T>(TrackedPool<T> pool) where T : Component;
    public void UnregisterPool<T>() where T : Component;
    public TrackedPool<T> GetPool<T>() where T : Component;
}
