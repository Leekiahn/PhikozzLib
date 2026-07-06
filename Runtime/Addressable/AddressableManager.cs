using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace PhikozzLib
{
    public class AddressableManager : MonoBehaviour, IAddressableService, IServiceRegister
    {
        private readonly Dictionary<string, object> _loadedAssets = new();
        private readonly Dictionary<string, AsyncOperationHandle> _handles = new();

        public void RegisterService()
        {
            ServiceLocator.Register<IAddressableService>(this);
        }

        public async UniTask<T> Load<T>(string key)
        {
            if (_loadedAssets.TryGetValue(key, out var cached))
            {
                return (T)cached;
            }

            var handle = Addressables.LoadAssetAsync<T>(key);

            await handle.ToUniTask();

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _loadedAssets[key] = handle.Result;
                _handles[key] = handle;
                return handle.Result;
            }
            else
            {
                Debug.LogError($"Failed to load asset with key: {key}");
                return default;
            }
        }

        public T Get<T>(string key) where T : UnityEngine.Object
        {
            if (_loadedAssets.TryGetValue(key, out var obj))
                return obj as T;

            return null;
        }

        // public async Task<IList<T>> LoadAllAsync<T>(AssetLabelReference labelReference)
        // {
        //     if (_loadedLabelAssets.TryGetValue(labelReference, out var existingHandle))
        //     {
        //         await existingHandle.Task;
        //         return (IList<T>)existingHandle.Result;
        //     }
        //     
        //     var handle = Addressables.LoadAssetsAsync<T>(labelReference, null);
        //     _loadedLabelAssets.Add(labelReference, handle);
        //     await handle.Task;
        //     return handle.Result;
        // }
        //
        //
        // public void Release(AssetReference assetReference)
        // {
        //     if (_loadedAssets.TryGetValue(assetReference, out var handle))
        //     {
        //         Addressables.Release(handle);
        //     }
        // }
        //
        // public void ReleaseLabel(AssetLabelReference labelReference)
        // {
        //     if (_loadedLabelAssets.TryGetValue(labelReference, out var handle))
        //     {
        //         Addressables.Release(handle);
        //     }
        // }
    }
}