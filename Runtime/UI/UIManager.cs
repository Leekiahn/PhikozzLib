using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PhikozzLib
{
    public class UIManager : MonoBehaviour, IUIService, IServiceRegister
    {
        private Transform _uiRoot;
        private readonly Dictionary<Type, UIBase> _prefabByType = new();
        private readonly Dictionary<Type, UIBase> _openedByType = new();

        public void RegisterService()
        {
            ServiceLocator.Register<IUIService>(this);
            _uiRoot = transform;
        }

        public async UniTask InitializeAsync()
        {
            await Core.Addressable.Load<GameObject>("UI");

            IList<GameObject> prefabs = Core.Addressable.GetAll<GameObject>("UI");
            foreach (GameObject prefab in prefabs)
            {
                UIBase ui = prefab.GetComponent<UIBase>();
                if (ui == null)
                {
                    continue;
                }

                _prefabByType[ui.GetType()] = ui;
            }
        }

        public T Open<T>() where T : UIBase
        {
            Type type = typeof(T);

            if (_openedByType.TryGetValue(type, out UIBase opened))
            {
                opened.Show();
                return (T)opened;
            }

            if (!_prefabByType.TryGetValue(type, out UIBase prefab))
            {
                throw new KeyNotFoundException($"{type.Name} UI prefab is not preloaded.");
            }

            T instance = Instantiate(prefab, _uiRoot) as T;
            if (instance == null)
            {
                throw new InvalidCastException($"{type.Name} prefab instantiate failed.");
            }

            _openedByType[type] = instance;
            instance.Show();
            return instance;
        }
    }
}