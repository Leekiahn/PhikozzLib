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
                Addressables.Release(pair.Value);
            }

            foreach (var pair in _labelHandles)
            {
                Addressables.Release(pair.Value);
            }

            _assetHandles.Clear();
            _labelHandles.Clear();
        }

        public void RegisterService()
        {
            ServiceLocator.Register<IResourceService>(this);
        }

        public async Task<T> LoadAsync<T>(string key)
        {
            if (_assetHandles.TryGetValue(key, out AsyncOperationHandle cachedHandle))
            {
                if (cachedHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    if (cachedHandle.Result is T cachedAsset)
                    {
                        return cachedAsset;
                    }

                    throw new InvalidOperationException(
                        $"Key '{key}' 에 해당하는 캐시된 에셋이 있지만, 타입이 일치하지 않습니다. " +
                        $"캐시된 타입: {cachedHandle.Result.GetType().Name}, 요청된 타입: {typeof(T).Name}");
                }
            }

            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key);
            await handle.Task;
            _assetHandles[key] = handle;

            return handle.Result;
        }

        public async Task<IList<T>> LoadAllAsync<T>(string label)
        {
            if (_labelHandles.TryGetValue(label, out AsyncOperationHandle cachedHandle))
            {
                if (cachedHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    if (cachedHandle.Result is IList<T> cachedAssets)
                    {
                        return cachedAssets;
                    }
                    
                    throw new InvalidOperationException(
                        $"Label '{label}' 에 해당하는 캐시된 에셋 목록이 있지만, 타입이 일치하지 않습니다. " +
                        $"캐시된 타입: {cachedHandle.Result.GetType().Name}, 요청된 타입: IList<{typeof(T).Name}>");
                }
                
            }

            AsyncOperationHandle<IList<T>> handle = Addressables.LoadAssetsAsync<T>(label, null);
            await handle.Task;
            _labelHandles[label] = handle;

            return handle.Result;
        }

        public void Release(string key)
        {
            if (_assetHandles.TryGetValue(key, out AsyncOperationHandle handle))
            {
                Addressables.Release(handle);
                _assetHandles.Remove(key);
            }
        }

        public void ReleaseAll(string label)
        {
            if (_labelHandles.TryGetValue(label, out AsyncOperationHandle handle))
            {
                Addressables.Release(handle);
                _labelHandles.Remove(label);
            }
        }
    }
}