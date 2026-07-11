using System.Collections.Generic;
using System.Linq;
using PhikozzLib;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.ResourceLocations;

public class AddressableManager : MonoBehaviour, IAddressableService, IServiceRegister
{
    private class LabelCache
    {
        public AsyncOperationHandle<IList<IResourceLocation>> LocationsHandle;
        public readonly Dictionary<string, IResourceLocation> Locations = new();
        public readonly Dictionary<string, AsyncOperationHandle> Handles = new();
        public readonly Dictionary<string, Object> LoadedAssets = new();
    }   
    
    private readonly Dictionary<string, LabelCache> _labelCaches = new();
    
    public void RegisterService()
    {
        ServiceLocator.Register<IAddressableService>(this);
    }

    public async UniTask PreloadLocations(string label)
    {
        if (_labelCaches.ContainsKey(label))
        {
            return;
        }
        
        var cache = new LabelCache();
        
        cache.LocationsHandle = Addressables.LoadResourceLocationsAsync(label);
        await cache.LocationsHandle.ToUniTask();
        
        foreach (var location in cache.LocationsHandle.Result)
        {
            cache.Locations[location.PrimaryKey] = location;
        }
        
        _labelCaches[label] = cache;
    }

    public async UniTask PreloadAssets<T>(string label) where T : Object
    {
        var cache = _labelCaches[label];

        foreach (var location in cache.Locations)
        {
            if (cache.LoadedAssets.ContainsKey(location.Key))
            {
                continue;
            }
            
            var handle = Addressables.LoadAssetAsync<T>(location.Value);
            cache.Handles[location.Key] = handle;
            var result = await handle.ToUniTask();
            cache.LoadedAssets[location.Key] = result;
        }
    }

    public T Get<T>(string label, string key) where T : Object
    {
        var cache = _labelCaches[label];
        
        if (cache.LoadedAssets.TryGetValue(key, out var loadedAsset))
        {
            return loadedAsset as T;
        }
        
        return null;
    }

    public IReadOnlyList<T> GetAll<T>(string label) where T : Object
    {
        return _labelCaches[label].LoadedAssets.Values.OfType<T>().ToList();
    }

    public void Release(string label, string key)
    {
        var cache = _labelCaches[label];

        if (cache.Handles.TryGetValue(key, out var handle))
        {
            Addressables.Release(handle);
            
            cache.Handles.Remove(key);
            cache.LoadedAssets.Remove(key);
        }
    }

    public void ReleaseAll(string label)
    {
        var cache = _labelCaches[label];

        foreach (var handle in cache.Handles.Values)
        {
            Addressables.Release(handle);
        }
        
        cache.Handles.Clear();
        cache.LoadedAssets.Clear();
    }

}
