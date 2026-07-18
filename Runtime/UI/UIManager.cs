using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PhikozzLib
{
    public class UIManager : MonoBehaviour, IUIService, IServiceRegister
    {
        private readonly Dictionary<Type, UIHUD> _registeredHUDs = new();

        public void RegisterService()
        {
            ServiceLocator.Register<IUIService>(this);
        }

        
        
        public void RegisterHUD<T>(T uiHud) where T : UIHUD
        {
            if (!_registeredHUDs.ContainsKey(typeof(T)))
            {
                _registeredHUDs.Add(typeof(T), uiHud);
            }
        }

        public void UnregisterHUD<T>(T uihud) where T : UIHUD
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
        
        public void HideAll()
        {
            foreach (var instance in _registeredHUDs.Values)
            {
                if (instance.IsVisible)
                {
                    instance.Hide();
                }
            }
        }
    }
}