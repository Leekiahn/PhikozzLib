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

            // 모든 매니저의 RegisterService()가 끝난 뒤에만 Init()을 호출한다.
            // Awake()에서 다른 서비스를 조회하면 BootstrapConfig의 매니저 순서에 의존하는 버그가 생긴다.
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