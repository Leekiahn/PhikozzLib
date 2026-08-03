using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PhikozzLib
{
    public class UIManager : MonoBehaviour, IUIService, IServiceRegister
    {
        private const string UIConfigResourcePath = "UIConfig";
        private UIConfig _uiConfig;
        
        [SerializeField] private Transform _windowParent;
        [SerializeField] private Transform _overlayParent;

        private readonly Dictionary<Type, UIWindow> _windows = new();
        private readonly Dictionary<Type, UIWindow> _openedWindows = new();
        private readonly Dictionary<Type, UIOverlay> _overlays = new();
        private readonly Dictionary<Type, UIOverlay> _openedOverlays = new();


        private async void Awake()
        {
            _uiConfig = Resources.Load<UIConfig>(UIConfigResourcePath);
            
            try
            {
                await PreLoad();
            }
            catch (Exception e)
            {
                throw new Exception(
                    $"Failed to load UI prefabs with labels: {_uiConfig.WindowLabelReference.labelString}, {_uiConfig.OverlayLabelReference.labelString}",
                    e);
            }
        }

        public void RegisterService()
        {
            ServiceLocator.Register<IUIService>(this);
        }

        private async UniTask PreLoad()
        {
            await LoadWindowPrefabs(_uiConfig.WindowLabelReference.labelString);
            await LoadOverlayPrefabs(_uiConfig.OverlayLabelReference.labelString);
        }

        public async UniTask LoadWindowPrefabs(string label)
        {
            await Core.Addressable.PreloadLocations<GameObject>(label);
            await Core.Addressable.PreloadAssets<GameObject>(label);

            var windowPrefabs = Core.Addressable.GetAll<GameObject>(label);

            foreach (var prefab in windowPrefabs)
            {
                var window = prefab.GetComponent<UIWindow>();
                _windows[window.GetType()] = window;
            }
        }

        public async UniTask LoadOverlayPrefabs(string label)
        {
            await Core.Addressable.PreloadLocations<GameObject>(label);
            await Core.Addressable.PreloadAssets<GameObject>(label);

            var overlayPrefabs = Core.Addressable.GetAll<GameObject>(label);

            foreach (var prefab in overlayPrefabs)
            {
                var overlay = prefab.GetComponent<UIOverlay>();
                _overlays[overlay.GetType()] = overlay;
            }
        }

        #region ---------------UIWindow---------------

        public T OpenWindow<T>() where T : UIWindow
        {
            if (!_windows.TryGetValue(typeof(T), out var prefab))
            {
                return null;
            }

            if (_openedWindows.TryGetValue(typeof(T), out var openedWindow))
            {
                if (!openedWindow.IsVisible)
                {
                    openedWindow.Open();
                }

                return (T)openedWindow;
            }

            var instance = Instantiate(prefab, _windowParent);
            instance.Open();
            _openedWindows[typeof(T)] = instance;
            return (T)instance;
        }

        public void CloseWindow<T>() where T : UIWindow
        {
            if (_openedWindows.TryGetValue(typeof(T), out var openedWindow))
            {
                openedWindow.Close();
            }
        }

        public void CloseWindow(UIWindow window)
        {
            var type = window.GetType();
            if (_openedWindows.TryGetValue(type, out var openedWindow))
            {
                openedWindow.Close();
            }
        }

        public void CloseAllWindow()
        {
            foreach (var openedWindow in _openedWindows.Values)
            {
                openedWindow.Close();
            }
        }

        #endregion


        #region --------------UIOverlay---------------

        public T OpenOverlay<T>() where T : UIOverlay
        {
            if (!_overlays.TryGetValue(typeof(T), out var prefab))
            {
                return null;
            }

            if (_openedOverlays.TryGetValue(typeof(T), out var openedOverlay))
            {
                if (!openedOverlay.IsVisible)
                {
                    openedOverlay.Show();
                }

                return (T)openedOverlay;
            }

            var instance = Instantiate(prefab, _overlayParent);
            instance.Show();
            _openedOverlays[typeof(T)] = instance;
            return (T)instance;
        }

        public void CloseOverlay<T>() where T : UIOverlay
        {
            if (_openedOverlays.TryGetValue(typeof(T), out var openedOverlay))
            {
                openedOverlay.Hide();
            }
        }

        public void CloseOverlay(UIOverlay overlay)
        {
            var type = overlay.GetType();
            if (_openedOverlays.TryGetValue(type, out var openedOverlay))
            {
                openedOverlay.Hide();
            }
        }

        public void CloseAllOverlay()
        {
            foreach (var openedOverlay in _openedOverlays.Values)
            {
                openedOverlay.Hide();
            }
        }

        #endregion
    }
}