using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace PhikozzLib
{
    public interface IAddressableService
    {
        UniTask PreloadLocations<T>(string label) where T : Object;
        UniTask PreloadAssets<T>(string label) where T : Object;
        T Get<T>(string label, string key) where T : Object;
        IReadOnlyList<T> GetAll<T>(string label) where T : Object;
        void Release(string label, string key);
        void ReleaseAll(string label);
    }
}