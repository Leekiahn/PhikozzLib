using System;
using System.Collections.Generic;

namespace PhikozzLib
{
    public static class ServiceLocator
    {
        private static Dictionary<Type, object> _services = new();

        public static void Register<T>(T service) where T : class
        {
            _services[typeof(T)] = service;
        }
        
        public static void Unregister<T>() where T : class
        {
            _services.Remove(typeof(T));
        }
    
        public static T Get<T>() where T : class
        {
            if (_services.TryGetValue(typeof(T), out var service))
            {
                return service as T;
            }
        
            throw new Exception($"{typeof(T).Name} : it is not registered");
        }
    }
}
