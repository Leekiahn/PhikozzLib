using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PhikozzLib
{
    public class SceneLoadManager : MonoBehaviour, ISceneService, IServiceRegister
    {
        public MMAdditiveSceneLoadingManagerSettings settings;

        private AsyncOperation _preloadedSceneHandle;
        private string _preloadedSceneName;

        public void RegisterService()
        {
            ServiceLocator.Register<ISceneService>(this);
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }

        public void LoadSceneWithLoading(string sceneName, string loadingSceneName)
        {
            MMSceneLoadingManager.LoadScene(sceneName, loadingSceneName);
        }

        public void LoadAdditiveScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }

        public void LoadAdditiveSceneWithLoading(string sceneName, string loadingSceneName)
        {
            settings.LoadingSceneName = loadingSceneName;
            MMAdditiveSceneLoadingManager.LoadScene(sceneName, settings);
        }

        public AsyncOperation LoadSceneAsync(string sceneName)
        {
            AsyncOperation handle = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            return handle;
        }

        public AsyncOperation LoadAdditiveSceneAsync(string sceneName)
        {
            AsyncOperation handle = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            return handle;
        }

        public AsyncOperation PreloadSceneAsync(string sceneName)
        {
            AsyncOperation handle = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            if (handle != null)
            {
                handle.allowSceneActivation = false;
                _preloadedSceneHandle = handle;
                _preloadedSceneName = sceneName;
            }

            return handle;
        }

        public AsyncOperation GetPreloadedSceneHandle()
        {
            if (_preloadedSceneHandle != null)
            {
                return _preloadedSceneHandle;
            }

            return null;
        }

        public void ActivatePreloadedScene()
        {
            if (_preloadedSceneHandle == null)
            {
                return;
            }

            AsyncOperation preloadedSceneHandle = _preloadedSceneHandle;
            Scene previousActiveScene = SceneManager.GetActiveScene();

            preloadedSceneHandle.completed += _ =>
            {
                Scene loadedScene = SceneManager.GetSceneByName(_preloadedSceneName);

                if (loadedScene.isLoaded)
                {
                    SceneManager.SetActiveScene(loadedScene);
                }

                SceneManager.UnloadSceneAsync(previousActiveScene);
            };

            _preloadedSceneHandle = null;
            _preloadedSceneName = null;
            preloadedSceneHandle.allowSceneActivation = true;
        }

        public AsyncOperation UnloadSceneAsync(string sceneName)
        {
            AsyncOperation handle = SceneManager.UnloadSceneAsync(sceneName);
            return handle;
        }

        public void SetHold(eSceneLoadingHoldMode holdMode, bool status)
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