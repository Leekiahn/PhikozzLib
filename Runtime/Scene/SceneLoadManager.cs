using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using PhikozzLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PhikozzLib
{
    public class SceneLoadManager : MonoBehaviour, ISceneService, IServiceRegister
    {
        public MMAdditiveSceneLoadingManagerSettings settings;
        
        public void RegisterService()
        {
            ServiceLocator.Register<ISceneService>(this);
        }

        public void Load(string sceneName, string loadingSceneName)
        {
            MMSceneLoadingManager.LoadScene(sceneName, loadingSceneName);
        }

        public void LoadAdditive(string sceneName, string loadingSceneName)
        {
            settings.LoadingSceneName = loadingSceneName;
            MMAdditiveSceneLoadingManager.LoadScene(sceneName, settings);
        }
        
        public void HoldSceneLoading(eSceneLoadingHoldMode holdMode, bool status)
        {
            MMAdditiveSceneLoadingManager.HoldModes mode = default;
            
            switch (holdMode)
            {
                case eSceneLoadingHoldMode.BeforeExitFade:
                    mode = MMAdditiveSceneLoadingManager.HoldModes.BeforeExitFade;
                    break;

                case eSceneLoadingHoldMode.AfterEntryFade:
                    mode = MMAdditiveSceneLoadingManager.HoldModes.AfterEntryFade;
                    break;

                case eSceneLoadingHoldMode.AfterUnloadOriginScene:
                    mode = MMAdditiveSceneLoadingManager.HoldModes.AfterUnloadOriginScene;
                    break;

                case eSceneLoadingHoldMode.BeforeSceneActivation:
                    mode = MMAdditiveSceneLoadingManager.HoldModes.BeforeSceneActivation;
                    break;
            }
            
            MMAdditiveSceneLoadingManager.SetHold(mode, status);
        }
    }
}