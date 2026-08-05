using System;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace PhikozzLib
{
    public class FloatingTextManager : MonoBehaviour, IServiceRegister, IFloatingTextService
    {
        [Serializable]
        private class SpawnerData
        {
            [SerializeField] private string _key;
            [SerializeField] private MMFloatingTextSpawner _spawner;
            
            public string Key => _key;
            public MMFloatingTextSpawner Spawner => _spawner;
        }

        [SerializeField] private List<SpawnerData> _spawners = new();

        private readonly Dictionary<string, MMFloatingTextSpawner> _floatingTextSpawnersDict = new();

        private void Awake()
        {
            foreach (var spawnerData in _spawners)
            {
                GameObject spawnerObject = Instantiate(spawnerData.Spawner.gameObject, transform);
                MMFloatingTextSpawner spawner = spawnerObject.GetComponent<MMFloatingTextSpawner>();
                _floatingTextSpawnersDict[spawnerData.Key] = spawner;
            }
        }

        public void RegisterService()
        {
            ServiceLocator.Register<IFloatingTextService>(this);
        }

        public void Spawn(string key, string value, Vector3 position, Vector3 direction)
        {
            if (_floatingTextSpawnersDict.TryGetValue(key, out var spawner))
            {
                spawner.Spawn(value, position, direction);
            }
        }
    }
}