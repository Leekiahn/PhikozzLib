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

            if (!_uiPrefabs.TryGetValue(type, out UIBase prefab))
            {
                throw new InvalidOperationException(
                    $"{type.Name} UI 프리팹이 `UIManager` 인스펙터에 등록되지 않았습니다.");
            }

            UIBase instance = Instantiate(prefab, _uiRoot);
            T ui = instance.GetComponentInChildren<T>(true);

            if (ui == null)
            {
                Destroy(instance.gameObject);
                throw new InvalidOperationException(
                    $"{type.Name} 타입의 UI 컴포넌트를 프리팹에서 찾을 수 없습니다.");
            }

            _openedUIs[type] = ui;
            ui.Show();

            return ui;
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
                if (reference == null || reference.Prefab == null)
                {
                    continue;
                }

                Type type = reference.Prefab.GetType();

                if (_uiPrefabs.ContainsKey(type))
                {
                    Debug.LogWarning($"{type.Name} UI 프리팹이 중복 등록되어 있습니다.", this);
                    continue;
                }

                _uiPrefabs.Add(type, reference.Prefab);
            }
        }
    }
}