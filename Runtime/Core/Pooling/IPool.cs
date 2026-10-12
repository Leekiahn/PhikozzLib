using UnityEngine;

namespace PhikozzLib
{
    public interface IPool
    {
        Component Get();
        void Release(Component instance);
        void ReleaseAll();
        void Clear();
    }
}
