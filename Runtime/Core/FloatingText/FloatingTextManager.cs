using System;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using PhikozzLib;
using UnityEngine;

namespace PhikozzLib
{
    public class FloatingTextManager : MonoBehaviour, IServiceRegister, IFloatingTextService
    {
        private readonly Dictionary<string, MMFloatingTextSpawner> _floatingTextSpawners = new();


        public void RegisterService()
        {
            ServiceLocator.Register<IFloatingTextService>(this);
        }

        public void RegisterFloatingText(BaseFloatingTextLoader loader)
        {
            _floatingTextSpawners[loader.FloatingTextKey] = loader.Spawner;
        }

        public void UnRegisterFloatingText(string key)
        {
            _floatingTextSpawners.Remove(key);
        }

        public void Spawn(string key, string value, Vector3 position, Vector3 direction)
        {
            if (_floatingTextSpawners.TryGetValue(key, out var spawner))
            {
                spawner.Spawn(value, position, direction);
            }
        }
    }
}
