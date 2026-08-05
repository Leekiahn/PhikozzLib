using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace PhikozzLib
{
    public class AddressableManager : MonoBehaviour, IAddressableService, IServiceRegister
    {
        private class LabelCache
        {
            public AsyncOperationHandle<IList<IResourceLocation>> LocationsHandle { get; }
            public Dictionary<string, IResourceLocation> LocationByKey { get; }
            public Dictionary<string, AsyncOperationHandle> HandleByKey { get; }
            public Dictionary<string, Object> AssetByKey { get; }

            public LabelCache(
                AsyncOperationHandle<IList<IResourceLocation>> locationsHandle,
                Dictionary<string, IResourceLocation> locationByKey,
                Dictionary<string, AsyncOperationHandle> handleByKey,
                Dictionary<string, Object> assetByKey
                )
            {
                LocationsHandle = locationsHandle;
                LocationByKey = locationByKey;
                HandleByKey = handleByKey;
                AssetByKey = assetByKey;
            }
        }

        private readonly Dictionary<string, LabelCache> _labelCaches = new();

        public void RegisterService()
        {
            ServiceLocator.Register<IAddressableService>(this);
        }
        
        public async UniTask DownloadDependencies(string label)
        {
            var sizeHandle = Addressables.GetDownloadSizeAsync(label);

            long size = await sizeHandle.ToUniTask();

            Addressables.Release(sizeHandle);

            if (size <= 0)
                return;

            var downloadHandle = Addressables.DownloadDependenciesAsync(label);

            await downloadHandle.ToUniTask();

            Addressables.Release(downloadHandle);
        }

        public async UniTask PreloadLocations<T>(string label) where T : Object
        {
            if (_labelCaches.ContainsKey(label))
            {
                return;
            }

            var cache = new LabelCache(
                Addressables.LoadResourceLocationsAsync(label, typeof(T)),
                new Dictionary<string, IResourceLocation>(),
                new Dictionary<string, AsyncOperationHandle>(),
                new Dictionary<string, Object>()
            );

            await cache.LocationsHandle.ToUniTask();

            foreach (var location in cache.LocationsHandle.Result)
            {
                cache.LocationByKey[location.PrimaryKey] = location;
            }

            _labelCaches[label] = cache;
        }

        public async UniTask PreloadAssets<T>(string label) where T : Object
        {
            if (!_labelCaches.TryGetValue(label, out var cache))
            {
                await PreloadLocations<T>(label);
                cache = _labelCaches[label];
            }

            var tasks = new List<UniTask>();

            foreach (var location in cache.LocationByKey)
            {
                if (cache.AssetByKey.ContainsKey(location.Key))
                    continue;

                tasks.Add(LoadAsset<T>(cache, location.Key, location.Value));
            }

            await UniTask.WhenAll(tasks);
        }
        
        public bool IsLoadedAssetKey(string label, string key)
        {
            return _labelCaches.TryGetValue(label, out var cache)
                   && cache.AssetByKey.ContainsKey(key);
        }
        
        public bool IsCachedLabel(string label)
        {
            return _labelCaches.ContainsKey(label);
        }

        public T Get<T>(string label, string key) where T : Object
        {
            var cache = _labelCaches[label];

            if (cache.AssetByKey.TryGetValue(key, out var loadedAsset))
            {
                return loadedAsset as T;
            }
         
            return null;
        }

        public IReadOnlyList<T> GetAll<T>(string label) where T : Object
        {
            return _labelCaches[label].AssetByKey.Values.OfType<T>().ToList();
        }

        public void Release(string label, string key)
        {
            var cache = _labelCaches[label];

            if (cache.HandleByKey.TryGetValue(key, out var handle))
            {
                Addressables.Release(handle);

                cache.HandleByKey.Remove(key);
                cache.AssetByKey.Remove(key);
            }
        }

        public void ReleaseAll(string label)
        {
            var cache = _labelCaches[label];

            foreach (var handle in cache.HandleByKey.Values)
            {
                Addressables.Release(handle);
            }

            cache.HandleByKey.Clear();
            cache.AssetByKey.Clear();
        }

        private async UniTask LoadAsset<T>(
            LabelCache cache,
            string key,
            IResourceLocation location) where T : Object
        {
            var handle = Addressables.LoadAssetAsync<T>(location);

            cache.HandleByKey[key] = handle;

            var asset = await handle.ToUniTask();

            cache.AssetByKey[key] = asset;
        }
    }
}
