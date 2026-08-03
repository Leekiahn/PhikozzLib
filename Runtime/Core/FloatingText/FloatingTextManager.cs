using System;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using PhikozzLib;
using UnityEngine;

namespace PhikozzLib
{
    public class FloatingTextManager : MonoBehaviour, IServiceRegister, IFloatingTextService
    {
        private readonly Dictionary<eFloatingTextType, MMFloatingTextSpawner> _floatingTextSpawners = new();


        public void RegisterService()
        {
            ServiceLocator.Register<IFloatingTextService>(this);
        }

        public void RegisterFloatingText(BaseFloatingTextLoader loader)
        {
            _floatingTextSpawners[loader.FloatingTextType] = loader.Spawner;
        }

        public void UnRegisterFloatingText(eFloatingTextType type)
        {
            _floatingTextSpawners.Remove(type);
        }

        public void Spawn(eFloatingTextType type, string value, Vector3 position, Vector3 direction)
        {
            if (_floatingTextSpawners.TryGetValue(type, out var spawner))
            {
                spawner.Spawn(value, position, direction);
            }
        }
    }
}
