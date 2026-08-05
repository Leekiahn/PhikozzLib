using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PhikozzLib
{
    public class DataManager : MonoBehaviour, IDataService, IServiceRegister
    {
        private readonly Dictionary<Type, DataContainer<BaseData>> _dataContainers = new();

        public void RegisterService()
        {
            ServiceLocator.Register<IDataService>(this);
        }

        public void AddDataContainer<T>(DataContainer<T> container) where T : BaseData
        {
            _dataContainers[typeof(T)] = container as DataContainer<BaseData>;
        }
        
        public DataContainer<T> GetDataContainer<T>() where T : BaseData
        {
            if (_dataContainers.TryGetValue(typeof(T), out var container))
            {
                return container as DataContainer<T>;
            }
            return null;
        }
    }
}
