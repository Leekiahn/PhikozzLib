using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PhikozzLib
{
    public class DialogManager : MonoBehaviour, IDialogService, IServiceRegister
    {
        [SerializeField] private AssetLabelReference _dialogLabelReference;

        private readonly Dictionary<Type, UIDialog> _dialogPrefabs = new();
        private UIDialog _openedDialog;

        private Transform _dialogParent;

        private async void Awake()
        {
            try
            {
                _dialogParent = transform;
                await PreLoad();
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to load dialog prefabs with label: {_dialogLabelReference.labelString}", e);
            }
        }

        public void RegisterService()
        {
            ServiceLocator.Register<IDialogService>(this);
        }

        public async UniTask PreLoad()
        {
            await LoadDialogPrefabs(_dialogLabelReference.labelString);
        }

        public async UniTask LoadDialogPrefabs(string label)
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

        public T Show<T>(string text, float typingDuration) where T : UIDialog
        {
            if (_dialogPrefabs.TryGetValue(typeof(T), out var prefab))
            {
                var instance = Instantiate(prefab, _dialogParent);
                _openedDialog = instance;
                instance.Show(text, typingDuration);
                return (T)instance;
            }

            return null;
        }

        public T ShowNext<T>(string text, float typingDuration) where T : UIDialog
        {
            _openedDialog.Show(text, typingDuration);
            return (T)_openedDialog;
        }

        public void Hide<T>() where T : UIDialog
        {
            _openedDialog.Hide();
            _openedDialog = null;
        }
    }
}