using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PhikozzLib
{
    public class PopupManager : MonoBehaviour, IPopupService, IServiceRegister
    { 
        [SerializeField] private AssetLabelReference _popupLabelReference;
    
        private readonly Dictionary<Type, UIPopup> _popupPrefabs = new();
        private readonly Dictionary<Type, UIPopup> _openedPopup = new();
        
        private Transform _popupParent;

        private async void Awake()
        {
            try
            {
                _popupParent = transform;
                await PreLoad();
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to load popup prefabs with label: {_popupLabelReference.labelString}", e);
            }
        }

        public void RegisterService()
        {
            ServiceLocator.Register<IPopupService>(this);
        }
        
        public async UniTask PreLoad()
        {
            await LoadPopupPrefabs(_popupLabelReference.labelString);
        }
        
        public async UniTask LoadPopupPrefabs(string label)
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
        
        public T Open<T>() where T : UIPopup
        {
            if (_popupPrefabs.TryGetValue(typeof(T), out var prefab))
            {
                var instance = Instantiate(prefab, _popupParent);
                _openedPopup[typeof(T)] = instance;
                instance.Open();
                return (T)instance;
            }

            return null;
        }
        
        public void Close<T>() where T : UIPopup
        {
            if (_openedPopup.TryGetValue(typeof(T), out var instance))
            {
                instance.Close();
                _openedPopup.Remove(typeof(T));
            }
        }
        
        public void CloseAll()
        {
            foreach (var instance in _openedPopup.Values)
            {
                instance.Close();
            }

            _openedPopup.Clear();
        }
    }
}

