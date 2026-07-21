using UnityEngine.SceneManagement;
using System;
using UnityEngine;

namespace PhikozzLib
{
    public interface ISceneService
    {
        void Load(string sceneName, string loadingSceneName);
        void LoadAdditive(string sceneName, string loadingSceneName);
        public void HoldSceneLoading(eSceneLoadingHoldMode holdMode, bool status);
    }
}
