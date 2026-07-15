using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace PhikozzLib
{
    public class UIManager : MonoBehaviour, IUIService, IServiceRegister
    {
        private Transform _popupParent;
        [SerializeField] private AssetLabelReference _popupLabelReference;

        public void RegisterService()
        {
            ServiceLocator.Register<IUIService>(this);

            _popupParent = transform;
        }

        public async UniTask PreLoad(string label)
        {
        }

        public T Open<T>() where T : UIBase
        {
            return null;
        }

        public void Close<T>() where T : UIBase
        {
        }

        public void CloseAll()
        {
        }
    }
}