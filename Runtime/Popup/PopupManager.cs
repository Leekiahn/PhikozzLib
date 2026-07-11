using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace PhikozzLib
{
    public class PopupManager : MonoBehaviour, IPopupService, IServiceRegister
    {
        private Transform _popupParent;

        public void RegisterService()
        {
            ServiceLocator.Register<IPopupService>(this);

            _popupParent = transform;
        }

        public async UniTask PreLoad(string label)
        {
        }

        public T Open<T>() where T : UIPopup
        {
            return null;
        }

        public void Close<T>() where T : UIPopup
        {
        }

        public void CloseAll()
        {
        }
    }
}