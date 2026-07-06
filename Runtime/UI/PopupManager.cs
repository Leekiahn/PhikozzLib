using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PhikozzLib
{
    public class PopupManager : MonoBehaviour, IPopupService, IServiceRegister
    {
        private readonly Dictionary<Type, UIPopup> _prefabByType = new();
        private readonly Dictionary<Type, UIPopup> _openedPopupByType = new();

        private Transform _popupParent;

        public void RegisterService()
        {
            ServiceLocator.Register<IPopupService>(this);
        }

        public async UniTask Load(string label)
        {
            await Core.Addressable.Load<GameObject>(label);
            
            var popupPrefabs = Core.Addressable.GetAll<GameObject>(label);
            foreach (var popupPrefab in popupPrefabs)
            {
                var popupComponent = popupPrefab.GetComponent<UIPopup>();
                var type = popupPrefab.GetType();
                _prefabByType.Add(type, popupComponent);
            }

            _popupParent = transform;
        }

        public T Open<T>() where T : UIPopup
        {
            if (_prefabByType.TryGetValue(typeof(T), out UIPopup popup))
            {
                if (_openedPopupByType.TryGetValue(typeof(T), out UIPopup openedPopup))
                {
                    return (T)openedPopup;
                }

                openedPopup.Open();
                _openedPopupByType.Add(typeof(T), openedPopup);
                return (T)openedPopup;
            }
            
            throw new KeyNotFoundException($"{typeof(T).Name} UI prefab is not preloaded.");
        }

        public void Close<T>() where T : UIPopup
        {
            if (_openedPopupByType.TryGetValue(typeof(T), out UIPopup openedPopup))
            {
                openedPopup.Close();
                _openedPopupByType.Remove(typeof(T));
            }
            else
            {
                throw new KeyNotFoundException($"{typeof(T).Name} UI is not opened.");
            }
        }

        public void CloseAll()
        {
            foreach (var openedPopup in _openedPopupByType.Values)
            {
                openedPopup.Close();
            }

            _openedPopupByType.Clear();
        }
    }
}