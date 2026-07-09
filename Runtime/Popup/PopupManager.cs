using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PhikozzLib
{
    public class PopupManager : MonoBehaviour, IPopupService, IServiceRegister
    {
        private readonly Dictionary<Type, GameObject> _popupInstancesByType = new();
        private readonly Dictionary<Type, GameObject> _popupPrefabsByType = new();

        private Transform _popupParent;

        public void RegisterService()
        {
            ServiceLocator.Register<IPopupService>(this);

            _popupParent = transform;
        }

        public async UniTask Load(string label)
        {
            await Core.Addressable.Load<GameObject>(label);

            var prefabs = Core.Addressable.GetAll<GameObject>(label);

            foreach (var prefab in prefabs)
            {
                var popupType = prefab.GetComponent<UIPopup>().GetType();
                _popupPrefabsByType[popupType] = prefab;
            }
        }

        public T Open<T>() where T : UIPopup
        {
            var popupType = typeof(T);

            if (_popupInstancesByType.TryGetValue(popupType, out var existingPopup))
            {
                if (existingPopup == null)
                {
                    _popupInstancesByType.Remove(popupType);
                }
                else
                {
                    var existingPopupComponent = existingPopup.GetComponent<T>();

                    if (!existingPopup.activeSelf || !existingPopupComponent.IsOpened)
                    {
                        existingPopupComponent.Open();
                    }

                    return existingPopupComponent;
                }
            }

            if (_popupPrefabsByType.TryGetValue(popupType, out var popupPrefab))
            {
                var instance = Instantiate(popupPrefab, _popupParent);
                var popupComponent = instance.GetComponent<T>();

                _popupInstancesByType[popupType] = instance;
                popupComponent.Open();

                return popupComponent;
            }

            return null;
        }

        public void Close<T>() where T : UIPopup
        {
            var popupType = typeof(T);

            if (_popupInstancesByType.TryGetValue(popupType, out var popup) && popup != null)
            {
                popup.GetComponent<T>().Close();
            }
        }

        public void CloseAll()
        {
            foreach (var popup in _popupInstancesByType.Values)
            {
                if (popup == null)
                {
                    continue;
                }

                popup.GetComponent<UIPopup>().Close();
            }
        }
    }
}