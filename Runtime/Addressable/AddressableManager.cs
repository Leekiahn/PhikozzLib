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
        private readonly Dictionary<string, IList<string>> _keysByLabel = new();
        private readonly Dictionary<string, AsyncOperationHandle> _handleByLabel = new();
        
        public void RegisterService()
        {
            ServiceLocator.Register<IAddressableService>(this);
        }

        public async UniTask Load<T>(string label)
        {
            if (!_handleByLabel.ContainsKey(label))
            {
                var handle = Addressables.LoadAssetsAsync<T>(label);
                _handleByLabel.Add(label, handle);
                await handle.Task;
            }

            if (!_keysByLabel.ContainsKey(label))
            {
                var keys = await Addressables.LoadResourceLocationsAsync(label).Task;
                _keysByLabel.Add(label, new List<string>());
                foreach (var key in keys)
                {
                    _keysByLabel[label].Add(key.PrimaryKey);
                }
            }
        }


        public T Get<T>(string label, string key)
        {
            if (_keysByLabel.TryGetValue(label, out var cachedKeys))
            {
                if (cachedKeys.Contains(key))
                {
                    var handle = Addressables.LoadAssetAsync<T>(key);
                    return handle.WaitForCompletion();
                }
            }

            throw new KeyNotFoundException($"[{label}] 레이블은 등록되지 않았습니다.");
        }

        public IList<T> GetAll<T>(string label)
        {
            if (_handleByLabel.TryGetValue(label, out var cachedHandle))
            {
                return (IList<T>)cachedHandle.Result;
            }

            throw new KeyNotFoundException($"[{label}] 레이블은 등록되지 않았습니다.");
        }

        public void Release(string label)
        {
            if (_handleByLabel.TryGetValue(label, out var cachedHandle))
            {
                Addressables.Release(cachedHandle);
                return;
            }
            
            throw new KeyNotFoundException($"[{label}] 레이블은 등록되지 않았습니다.");
        }
    }
}