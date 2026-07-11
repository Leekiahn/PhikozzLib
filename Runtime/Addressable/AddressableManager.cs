using System.Collections.Generic;
using PhikozzLib;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;

public class AddressableManager : MonoBehaviour, IAddressableService, IServiceRegister
{
    private readonly Dictionary<string, AsyncOperationHandle> _loadedAssets = new Dictionary<string, AsyncOperationHandle>();
    private readonly Dictionary<string, AsyncOperationHandle> _loadedLabels = new Dictionary<string, AsyncOperationHandle>();
    
    
    public void RegisterService()
    {
        ServiceLocator.Register<IAddressableService>(this);
    }

    public async UniTask PreLoad(string label)
    {
    }
}
