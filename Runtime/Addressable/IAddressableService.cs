using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace PhikozzLib
{
    public interface IAddressableService
    {
        UniTask Load<T>(string label);
        T Get<T>(string label, string key);
        IList<T> GetAll<T>(string label);
        void Release(string label);
    }
}