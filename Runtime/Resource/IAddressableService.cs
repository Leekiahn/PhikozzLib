using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace PhikozzLib
{
    public interface IAddressableService
    {
        Task<T> LoadAsync<T>(AssetReference assetReference);
        Task<IList<T>> LoadAllAsync<T>(AssetLabelReference labelReference);
        void Release(AssetReference assetReference);
        void ReleaseLabel(AssetLabelReference labelReference);
    }
}