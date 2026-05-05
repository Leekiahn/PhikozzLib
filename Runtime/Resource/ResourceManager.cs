using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace PhikozzLib
{
    public class ResourceManager : MonoBehaviour, IResourceService, IServiceRegister
    {
        private readonly Dictionary<string, AsyncOperationHandle> _assetHandles = new();
        private readonly Dictionary<string, AsyncOperationHandle> _labelHandles = new();

        private void OnDestroy()
        {
            foreach (var pair in _assetHandles)
            {
                if (pair.Value.IsValid())
                {
                    Addressables.Release(pair.Value);
                }
            }

            foreach (var pair in _labelHandles)
            {
                if (pair.Value.IsValid())
                {
                    Addressables.Release(pair.Value);
                }
            }

            _assetHandles.Clear();
            _labelHandles.Clear();
        }
        
        public void RegisterService()
        {
            ServiceLocator.Register<IResourceService>(this);
        }

        public T Load<T>(string key)
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key);
            T asset = handle.WaitForCompletion();
            _assetHandles[key] = handle;

            return asset;
        }

        public async Task<T> LoadAsync<T>(string key)
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key);
            await handle.Task;
            _assetHandles[key] = handle;

            return handle.Result;
        }

        public IList<T> LoadAll<T>(string label)
        {
            AsyncOperationHandle<IList<T>> handle = Addressables.LoadAssetsAsync<T>(label, null);
            IList<T> assets = handle.WaitForCompletion();
            _labelHandles[label] = handle;
            
            return assets;
        }

        public async Task<IList<T>> LoadAllAsync<T>(string label)
        {
            AsyncOperationHandle<IList<T>> handle = Addressables.LoadAssetsAsync<T>(label, null);
            await handle.Task;
            _labelHandles[label] = handle;
            
            return handle.Result;
        }

        public void Release(string key)
        {
            if (_assetHandles.TryGetValue(key, out AsyncOperationHandle handle))
            {
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }

                _assetHandles.Remove(key);
            }
        }

        public void ReleaseAll(string label)
        {
            if (_labelHandles.TryGetValue(label, out AsyncOperationHandle handle))
            {
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }

                _labelHandles.Remove(label);
            }
        }
    }
}