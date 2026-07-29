using System;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using PhikozzLib;
using UnityEngine;

public class FloatingTextManager : MonoBehaviour, IServiceRegister
{
    [Serializable]
    private class FloatingTextData
    {
        [SerializeField] private eFloatingTextType _type;
        [SerializeField] private MMFloatingTextSpawner _spawner;

        public eFloatingTextType Type => _type;
        public MMFloatingTextSpawner Spawner => _spawner;
    }

    [SerializeField] private List<FloatingTextData> _floatingTextSpawners = new List<FloatingTextData>();


    public void RegisterService()
    {
        ServiceLocator.Register(this);
    }

    public void Spawn(eFloatingTextType type, string value, Vector3 position, Vector3 direction)
    {
        var data = _floatingTextSpawners.Find(x => x.Type == type);
        data.Spawner.Spawn(value, position, direction);
    }
}