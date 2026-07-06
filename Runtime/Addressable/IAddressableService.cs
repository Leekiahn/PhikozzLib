using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace PhikozzLib
{
    public interface IAddressableService
    {
        UniTask<T> Load<T>(string key);
        // Task<IList<T>> LoadAllAsync<T>(AssetLabelReference labelReference);
        // void Release(AssetReference assetReference);
        // void ReleaseLabel(AssetLabelReference labelReference);
    }
}