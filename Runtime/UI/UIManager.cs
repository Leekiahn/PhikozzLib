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
        [SerializeField] private AssetLabelReference _popupLabelReference;
        [SerializeField] private AssetLabelReference _dialogLabelReference;
        [SerializeField] private Transform _popupRoot;
        [SerializeField] private Transform _dialogRoot;

        private readonly Dictionary<Type, UIPopup> _popupPrefabs = new();
        private readonly Dictionary<Type, UIDialog> _dialogPrefabs = new();
        private readonly Dictionary<Type, UIPopup> _openedPopup = new();
        private readonly Dictionary<Type, UIDialog> _openedDialog = new();

        private readonly Dictionary<Type, UIHUD> _registeredHUDs = new();

        public void RegisterService()
        {
            ServiceLocator.Register<IUIService>(this);
        }

        public async UniTask PreLoad()
        {
            await LoadPopupPrefabs(_popupLabelReference.labelString);
            await LoadDialogPrefabs(_dialogLabelReference.labelString);
        }

        public void RegisterHUD<T>(T uiHud) where T : UIHUD
        {
            if (!_registeredHUDs.ContainsKey(typeof(T)))
            {
                _registeredHUDs.Add(typeof(T), uiHud);
            }
        }

        public void UnregisterHUD<T>() where T : UIHUD
        {
            if (_registeredHUDs.ContainsKey(typeof(T)))
            {
                _registeredHUDs.Remove(typeof(T));
            }
        }

        public T ShowHUD<T>() where T : UIHUD
        {
            if (_registeredHUDs.TryGetValue(typeof(T), out var instance))
            {
                if (!instance.IsVisible)
                {
                    instance.Show();
                    return (T)instance;
                }
            }

            return null;
        }

        public void HideHUD<T>() where T : UIHUD
        {
            if (_registeredHUDs.TryGetValue(typeof(T), out var instance))
            {
                if (instance.IsVisible)
                {
                    instance.Hide();
                }
            }
        }

        public T OpenPopup<T>() where T : UIPopup
        {
            if (_popupPrefabs.TryGetValue(typeof(T), out var prefab))
            {
                var instance = Instantiate(prefab, _popupRoot);
                _openedPopup[typeof(T)] = instance;
                instance.Open();
                return (T)instance;
            }

            return null;
        }

        public void ClosePopup<T>() where T : UIPopup
        {
            if (_openedPopup.TryGetValue(typeof(T), out var instance))
            {
                instance.Close();
                _openedPopup.Remove(typeof(T));
            }
        }

        public T ShowDialog<T>(string text, float typingDuration) where T : UIDialog
        {
            if (_dialogPrefabs.TryGetValue(typeof(T), out var prefab))
            {
                var instance = Instantiate(prefab, _dialogRoot);
                _openedDialog[typeof(T)] = instance;
                instance.Show(text, typingDuration);
                return (T)instance;
            }

            return null;
        }

        public void HideDialog<T>() where T : UIDialog
        {
            if (_openedDialog.TryGetValue(typeof(T), out var instance))
            {
                instance.Hide();
                _openedDialog.Remove(typeof(T));
            }
        }


        public void CloseAll()
        {
            foreach (var instance in _openedPopup.Values)
            {
                instance.Close();
            }

            _openedPopup.Clear();

            foreach (var instance in _openedDialog.Values)
            {
                instance.Hide();
            }

            _openedDialog.Clear();
        }

        private async UniTask LoadPopupPrefabs(string label)
        {
            await Core.Addressable.PreloadLocations<GameObject>(label);
            await Core.Addressable.PreloadAssets<GameObject>(label);

            var popupPrefabs = Core.Addressable.GetAll<GameObject>(label);

            foreach (var prefab in popupPrefabs)
            {
                var popup = prefab.GetComponent<UIPopup>();
                _popupPrefabs[popup.GetType()] = popup;
            }
        }

        private async UniTask LoadDialogPrefabs(string label)
        {
            await Core.Addressable.PreloadLocations<GameObject>(label);
            await Core.Addressable.PreloadAssets<GameObject>(label);

            var dialogPrefabs = Core.Addressable.GetAll<GameObject>(label);

            foreach (var prefab in dialogPrefabs)
            {
                var dialog = prefab.GetComponent<UIDialog>();
                _dialogPrefabs[dialog.GetType()] = dialog;
            }
        }
    }
}