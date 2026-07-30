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

            foreach (var manager in config.Managers)
            {
                var instance = Object.Instantiate(manager);
                Object.DontDestroyOnLoad(instance);

                var registration = instance.GetComponent<IServiceRegister>();
                registration.RegisterService();
            }
        }
    }
}