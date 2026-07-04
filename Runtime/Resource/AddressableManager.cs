using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace PhikozzLib
{
    public class AddressableManager : MonoBehaviour, IAddressableService, IServiceRegister
    {
        private readonly Dictionary<AssetReference, AsyncOperationHandle> _loadedAssets = new();
        private readonly Dictionary<AssetLabelReference, AsyncOperationHandle> _loadedLabelAssets = new();

        public void RegisterService()
        {
            ServiceLocator.Register<IAddressableService>(this);
        }
        
        public async Task<T> LoadAsync<T>(AssetReference assetReference)
        {
            if (_loadedAssets.TryGetValue(assetReference, out var existingHandle))
            {
                await existingHandle.Task;
                return (T)existingHandle.Result;
            }
            
            var handle = Addressables.LoadAssetAsync<T>(assetReference);
            _loadedAssets.Add(assetReference, handle);
            await handle.Task;
            return handle.Result;
        }

        public async Task<IList<T>> LoadAllAsync<T>(AssetLabelReference labelReference)
        {
            if (_loadedLabelAssets.TryGetValue(labelReference, out var existingHandle))
            {
                await existingHandle.Task;
                return (IList<T>)existingHandle.Result;
            }
            
            var handle = Addressables.LoadAssetsAsync<T>(labelReference, null);
            _loadedLabelAssets.Add(labelReference, handle);
            await handle.Task;
            return handle.Result;
        }


        public void Release(AssetReference assetReference)
        {
            if (_loadedAssets.TryGetValue(assetReference, out var handle))
            {
                Addressables.Release(handle);
                _loadedAssets.Remove(assetReference);
            }
        }
        
        public void ReleaseLabel(AssetLabelReference labelReference)
        {
            if (_loadedLabelAssets.TryGetValue(labelReference, out var handle))
            {
                Addressables.Release(handle);
                _loadedLabelAssets.Remove(labelReference);
            }
        }
    }
}