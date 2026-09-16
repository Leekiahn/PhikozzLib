using System.Collections.Generic;
using UnityEngine;

namespace PhikozzLib
{
    public static class Bootstrapper
    {
        private const string BootstrapConfigResourcePath = "BootstrapConfig";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            var config = Resources.Load<BootstrapConfig>(BootstrapConfigResourcePath);

            var instances = new List<GameObject>();

            foreach (var manager in config.Managers)
            {
                var instance = Object.Instantiate(manager);
                Object.DontDestroyOnLoad(instance);
                instances.Add(instance);

                var registration = instance.GetComponent<IServiceRegister>();
                registration.RegisterService();
            }

            foreach (var instance in instances)
            {
                if (instance.TryGetComponent<IServiceInit>(out var init))
                {
                    init.Init();
                }
            }
        }
    }
}