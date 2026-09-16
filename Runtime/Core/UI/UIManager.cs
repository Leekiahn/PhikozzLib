using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PhikozzLib
{
    public class UIManager : MonoBehaviour, IUIService, IServiceRegister, IServiceInit
    {
        [SerializeField] private AssetLabelReference _windowLabelReference;
        [SerializeField] private Transform _windowParent;

        private readonly Dictionary<Type, UIPopup> _popups = new();
        private readonly Dictionary<Type, UIPopup> _openedpopups = new();


        private IAddressableService _addressableService;

        public void RegisterService()
        {
            ServiceLocator.Register<IUIService>(this);
        }

        public void UnregisterService()
        {
            ServiceLocator.Unregister<IUIService>();
        }

        public async void Init()
        {
            _addressableService = ServiceLocator.Get<IAddressableService>();

            try
            {
                await PreLoad();
            }
            catch (Exception e)
            {
                throw new Exception(
                    $"Failed to load UI prefabs with labels: {_windowLabelReference.labelString}",
                    e);
            }
        }

        private async UniTask PreLoad()
        {
            await LoadPopupPrefabs(_windowLabelReference.labelString);
        }

        private async UniTask LoadPopupPrefabs(string label)
        {
            await _addressableService.PreloadLocations<GameObject>(label);
            await _addressableService.PreloadAssets<GameObject>(label);

            var popupPrefabs = _addressableService.GetAll<GameObject>(label);

            foreach (var prefab in popupPrefabs)
            {
                var popup = prefab.GetComponent<UIPopup>();
                _popups[popup.GetType()] = popup;
            }
        }


        #region ---------------UIWindow---------------

        public T OpenPopup<T>() where T : UIPopup
        {
            if (!_popups.TryGetValue(typeof(T), out var prefab))
            {
                Debug.LogWarning($"[UIManager] '{typeof(T).Name}' 팝업을 찾을 수 없습니다. 라벨 '{_windowLabelReference.labelString}'에 프리팹이 등록되어 있는지 확인하세요.");
                return null;
            }

            if (_openedpopups.TryGetValue(typeof(T), out var openedPopup))
            {
                if (!openedPopup.IsVisible)
                {
                    openedPopup.Open();
                }

                return (T)openedPopup;
            }

            var popupInstance = Instantiate(prefab, _windowParent);
            popupInstance.Init();
            popupInstance.Open();
            _openedpopups[typeof(T)] = popupInstance;
            return (T)popupInstance;
        }

        public void ClosePopup<T>() where T : UIPopup
        {
            if (_openedpopups.TryGetValue(typeof(T), out var openedPopup))
            {
                openedPopup.Close();
            }
        }

        public void ClosePopup(UIPopup window)
        {
            var type = window.GetType();
            if (_openedpopups.TryGetValue(type, out var openedPopup))
            {
                openedPopup.Close();
            }
        }

        public void CloseAllPopup()
        {
            foreach (var openedPopup in _openedpopups.Values)
            {
                openedPopup.Close();
            }
        }

        #endregion
    }
}