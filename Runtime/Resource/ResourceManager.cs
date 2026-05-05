using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;
using System.Linq;

namespace PhikozzLib
{
    public class ResourceManager : MonoBehaviour, IResourceService, IServiceRegister
    {
        public void RegisterService()
        {
            ServiceLocator.Register<IResourceService>(this);
        }

        public T Load<T>(string key)
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key);
            T asset = handle.WaitForCompletion();
            
            return asset;
        }

        public async Task<T> LoadAsync<T>(string key)
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key);
            await handle.Task;
            
            return handle.Result;
        }

        public List<T> LoadAll<T>(string label)
        {
            AsyncOperationHandle<IList<T>> handle = Addressables.LoadAssetsAsync<T>(label, null);
            IList<T> assets = handle.WaitForCompletion();
            
            return assets.ToList();
        }
        
        public async Task<List<T>> LoadAllAsync<T>(string label)
        {
            AsyncOperationHandle<IList<T>> handle = Addressables.LoadAssetsAsync<T>(label, null);
            await handle.Task;
            
            return handle.Result.ToList();
        }

        public void Release<T>(T asset)
        {
            Addressables.Release(asset);
        }
    }
}

