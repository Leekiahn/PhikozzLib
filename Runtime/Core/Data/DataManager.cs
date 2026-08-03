using System;
using System.Collections.Generic;
using UnityEngine;

namespace PhikozzLib
{
    public class DataManager : MonoBehaviour, IDataService, IServiceRegister
    {
        private readonly Dictionary<Type, object> _dataContainers = new();

        public void RegisterService()
        {
            ServiceLocator.Register(this);
        }

        public void Register<T>(DataContainer<T> container) where T : BaseData
        {
            _dataContainers[typeof(T)] = container;
        }
        
        public DataContainer<T> Get<T>() where T : BaseData
        {
            if (_dataContainers.TryGetValue(typeof(T), out var container))
            {
                return container as DataContainer<T>;
            }
            return null;
        }
    }
}
