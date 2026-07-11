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
        private readonly Dictionary<Type, GameObject> _openedPopupByType = new();
        private readonly Dictionary<Type, GameObject> _popupByType = new();
        private readonly Dictionary<string, AsyncOperationHandle<IList<GameObject>>> _handlesByLabel = new();

        private Transform _popupParent;

        public void RegisterService()
        {
            ServiceLocator.Register<IPopupService>(this);

            _popupParent = transform;
        }

        public async UniTask PreLoad(string label)
        {
            var handle = Addressables.LoadAssetsAsync<GameObject>(label);
            _handlesByLabel[label] = handle;
            var popupPrefabs = await handle.Task;

            foreach (var popup in popupPrefabs)
            {
                var instance = Instantiate(popup, _popupParent);
                var popupType = instance.GetComponent<UIPopup>().GetType();

                if (!_popupByType.ContainsKey(popupType))
                {
                    _popupByType.Add(popupType, instance);
                }
            }
            
            Addressables.Release(handle);
        }

        public T Open<T>() where T : UIPopup
        {
            var popupType = typeof(T);

            if (_openedPopupByType.TryGetValue(popupType, out var existingPopup))
            {
                return existingPopup.GetComponent<T>();
            }

            if (_popupByType.TryGetValue(popupType, out var popupPrefab))
            {
                var instance = Instantiate(popupPrefab, _popupParent);
                var popupComponent = instance.GetComponent<T>();

                _openedPopupByType[popupType] = instance;
                popupComponent.Open();

                return popupComponent;
            }

            return null;
        }

        public void Close<T>() where T : UIPopup
        {
            var popupType = typeof(T);

            if (_openedPopupByType.TryGetValue(popupType, out var popup) && popup != null)
            {
                popup.GetComponent<T>().Close();
                _openedPopupByType.Remove(popupType);
            }
        }

        public void CloseAll()
        {
            foreach (var popup in _openedPopupByType.Values)
            {
                if (popup == null)
                {
                    continue;
                }

                popup.GetComponent<UIPopup>().Close();
                _openedPopupByType.Remove(popup.GetComponent<UIPopup>().GetType());
            }
        }
    }
}