using UnityEngine;

namespace PhikozzLib
{
    public interface IPoolService
    {
        T Get<T>(T prefab) where T : MonoBehaviour, IPoolable;
        void Release<T>(T obj) where T : MonoBehaviour, IPoolable;
        void ReleaseAll<T>(T prefab) where T : MonoBehaviour, IPoolable;
        void DestroyAll<T>(T prefab) where T : MonoBehaviour, IPoolable;
    }
}