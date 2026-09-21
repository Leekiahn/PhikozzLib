using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace PhikozzLib
{
    public static class Bootstrapper
    {
        private const string BOOTSTRAP_CONFIG_RESOURCE_PATH = "BootstrapConfig";

        private static readonly UniTaskCompletionSource _readySource = new();

        public static UniTask WaitUntilReadyAsync()
        {
            return _readySource.Task;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            InitializeAsync().Forget();
        }

        private static async UniTaskVoid InitializeAsync()
        {
            var config = Resources.Load<BootstrapConfig>(BOOTSTRAP_CONFIG_RESOURCE_PATH);
            var instances = new List<GameObject>();
            var initTasks = new List<UniTask>();

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
                if (instance.TryGetComponent<IServiceInit>(out var serviceInit))
                {
                    initTasks.Add(serviceInit.InitAsync());
                }
            }

            await UniTask.WhenAll(initTasks);

            _readySource.TrySetResult();
        }
    }
}
