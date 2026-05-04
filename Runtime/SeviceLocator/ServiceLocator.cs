using System;
using System.Collections.Generic;

namespace PhikozzLib
{
    public static class ServiceLocator 
    {
        private static Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public static void Register<T>(T service) where T : class
        {
            _services[typeof(T)] = service;
        }
    
        public static T Get<T>() where T : class
        {
            if (_services.TryGetValue(typeof(T), out var service))
            {
                return service as T;
            }
        
            throw new Exception($"{typeof(T).Name} 서비스가 등록되지 않았습니다." + 
                                "ServiceLocator를 통해서 서비스를 등록해주세요.");
        }
    
        public static void Unregister<T>() where T : class
        {
            _services.Remove(typeof(T));
        }
    }
}
