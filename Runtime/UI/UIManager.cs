using System;
using System.Collections.Generic;
using UnityEngine;

namespace PhikozzLib
{
    public class UIManager : MonoBehaviour, IUIService, IServiceRegister
    {
        [Serializable]
        private class UIReference
        {
            [SerializeField] private UIBase _prefab;

            public UIBase Prefab => _prefab;
        }

        [SerializeField] private List<UIReference> _uiReferences = new();

        private Transform _uiRoot;
        private readonly Dictionary<Type, UIBase> _uiPrefabs = new();
        private readonly Dictionary<Type, UIBase> _openedUIs = new();

        public void RegisterService()
        {
            ServiceLocator.Register<IUIService>(this);
            
            _uiRoot = transform;
            BuildPrefabTable();
        }

        public T Open<T>() where T : UIBase
        {
            Type type = typeof(T);

            if (_openedUIs.TryGetValue(type, out UIBase cachedUI))
            {
                cachedUI.Show();
                return cachedUI as T;
            }

            if (_uiPrefabs.TryGetValue(type, out UIBase prefab))
            {
                UIBase instance = Instantiate(prefab, _uiRoot);
                T ui = instance.GetComponent<T>();

                _openedUIs[type] = ui;
                ui.Show();
                
                return ui;
            }

            return null;
        }

        public void Close<T>() where T : UIBase
        {
            Type type = typeof(T);

            if (_openedUIs.TryGetValue(type, out UIBase ui))
            {
                ui.Hide();
            }
        }

        private void BuildPrefabTable()
        {
            _uiPrefabs.Clear();

            foreach (UIReference reference in _uiReferences)
            {
                Type type = reference.Prefab.GetType();
                _uiPrefabs.Add(type, reference.Prefab);
            }
        }
    }
}