using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Sirenix.OdinInspector;

namespace PhikozzLib
{
    public class UIManager : MonoBehaviour, IUIService, IServiceRegister, IServiceInit
    {
        [SerializeField] private bool _loadByAddressableService;

        [ShowIf("_loadByAddressableService")]
        [SerializeField] private AssetLabelReference _popupLabelReference;
        [SerializeField] private Transform _popupParent;

        private readonly Dictionary<Type, UIPopup> _popups = new();
        private readonly Dictionary<Type, UIPopup> _openedPopups = new();


        private IAddressableService _addressableService;

        public void RegisterService()
        {
            ServiceLocator.Register<IUIService>(this);
        }

        public void UnregisterService()
        {
            ServiceLocator.Unregister<IUIService>();
        }

        public async UniTask InitAsync()
        {
            if (_loadByAddressableService)
            {
                _addressableService = ServiceLocator.Get<IAddressableService>();

                try
                {
                    await PreloadPopupByAddressableService();
                }
                catch (Exception e)
                {
                    throw new Exception(
                        $"Failed to load UI prefabs with labels: {_popupLabelReference.labelString}",
                        e);
                }
            }
        }

        private async UniTask PreloadPopupByAddressableService()
        {
            await _addressableService.PreloadLocations<GameObject>(_popupLabelReference.labelString);
            await _addressableService.PreloadAssets<GameObject>(_popupLabelReference.labelString);

            var popupPrefabs = _addressableService.GetAll<GameObject>(_popupLabelReference.labelString);

            foreach (var prefab in popupPrefabs)
            {
                var popup = prefab.GetComponent<UIPopup>();
                _popups[popup.GetType()] = popup;
            }
        }

        public void RegisterPopup(UIPopup prefab)
        {
            _popups[prefab.GetType()] = prefab;
        }

        public void UnregisterPopup(UIPopup prefab)
        {
            var popupType = prefab.GetType();

            if (_openedPopups.TryGetValue(popupType, out var openedPopup))
            {
                Destroy(openedPopup.gameObject);
                _openedPopups.Remove(popupType);
            }

            _popups.Remove(popupType);
        }

        public T OpenPopup<T>() where T : UIPopup
        {
            if (!_popups.TryGetValue(typeof(T), out var prefab))
            {
                Debug.LogWarning($"[UIManager] Popup '{typeof(T).Name}' is not registered.");
                return null;
            }

            if (_openedPopups.TryGetValue(typeof(T), out var openedPopup))
            {
                if (!openedPopup.IsVisible)
                {
                    openedPopup.Open();
                }

                return (T)openedPopup;
            }

            var popupInstance = Instantiate(prefab, _popupParent);
            popupInstance.Init();
            popupInstance.Open();
            _openedPopups[typeof(T)] = popupInstance;
            return (T)popupInstance;
        }

        public void ClosePopup<T>() where T : UIPopup
        {
            if (_openedPopups.TryGetValue(typeof(T), out var openedPopup))
            {
                openedPopup.Close();
            }
        }

        public void ClosePopup(UIPopup popup)
        {
            var type = popup.GetType();
            if (_openedPopups.TryGetValue(type, out var openedPopup))
            {
                openedPopup.Close();
            }
        }

        public void CloseAllPopup()
        {
            foreach (var openedPopup in _openedPopups.Values)
            {
                openedPopup.Close();
            }
        }
    }
}
