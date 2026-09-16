using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PhikozzLib.Editor
{
    [InitializeOnLoad]
    internal static class BootstrapConfigValidator
    {
        private const string BootstrapConfigResourcePath = "BootstrapConfig";

        static BootstrapConfigValidator()
        {
            Validate();
        }

        [MenuItem("PhikozzLib/Validate Bootstrap Config")]
        private static void ValidateFromMenu()
        {
            Validate();
        }

        private static void Validate()
        {
            var config = Resources.Load<BootstrapConfig>(BootstrapConfigResourcePath);

            if (config == null)
            {
                Debug.LogWarning($"[PhikozzLib] Resources/{BootstrapConfigResourcePath}.asset not found. Bootstrapper will not register any services.");
                return;
            }

            var seenPrefabs = new HashSet<GameObject>();

            for (var i = 0; i < config.Managers.Count; i++)
            {
                var manager = config.Managers[i];

                if (manager == null)
                {
                    Debug.LogWarning($"[PhikozzLib] BootstrapConfig.Managers[{i}] is empty. Bootstrapper will throw a NullReferenceException at runtime.");
                    continue;
                }

                if (!seenPrefabs.Add(manager))
                {
                    Debug.LogWarning($"[PhikozzLib] '{manager.name}' is registered more than once in BootstrapConfig.Managers.");
                }

                if (manager.GetComponent<IServiceRegister>() == null)
                {
                    Debug.LogWarning($"[PhikozzLib] '{manager.name}' has no component implementing IServiceRegister. Bootstrapper will throw a NullReferenceException at runtime.");
                }
            }
        }
    }
}
