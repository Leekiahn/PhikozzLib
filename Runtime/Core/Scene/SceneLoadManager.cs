using MoreMountains.Tools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PhikozzLib
{
    public class SceneLoadManager : MonoBehaviour, ISceneService, IServiceRegister
    {
        [SerializeField] private MMAdditiveSceneLoadingManagerSettings _settings;

        private AsyncOperation _preloadedSceneHandle;
        private string _preloadedSceneName;
        
        public void RegisterService()
        {
            ServiceLocator.Register<ISceneService>(this);
        }

        [PropertySpace(SpaceBefore = 30f)]
        [Button(ButtonSizes.Medium, ButtonStyle.Box)]
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }

        [Button(ButtonSizes.Medium, ButtonStyle.Box)]
        public void LoadSceneWithLoading(string sceneName, string loadingSceneName)
        {
            MMSceneLoadingManager.LoadScene(sceneName, loadingSceneName);
        }

        [Button(ButtonSizes.Medium, ButtonStyle.Box)]
        public void LoadAdditiveScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }

        [Button(ButtonSizes.Medium, ButtonStyle.Box)]
        public void LoadAdditiveSceneWithLoading(string sceneName, string loadingSceneName)
        {
            _settings.LoadingSceneName = loadingSceneName;
            MMAdditiveSceneLoadingManager.LoadScene(sceneName, _settings);
        }

        [Button(ButtonSizes.Medium, ButtonStyle.Box)]
        public AsyncOperation LoadSceneAsync(string sceneName)
        {
            AsyncOperation handle = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            return handle;
        }

        [Button(ButtonSizes.Medium, ButtonStyle.Box)]
        public AsyncOperation LoadAdditiveSceneAsync(string sceneName)
        {
            AsyncOperation handle = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            return handle;
        }

        [Button(ButtonSizes.Medium, ButtonStyle.Box)]
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

        [Button(ButtonSizes.Medium, ButtonStyle.Box)]
        public AsyncOperation GetPreloadedSceneHandle()
        {
            if (_preloadedSceneHandle != null)
            {
                return _preloadedSceneHandle;
            }

            return null;
        }

        [Button(ButtonSizes.Medium, ButtonStyle.Box)]
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

        [Button(ButtonSizes.Medium, ButtonStyle.Box)]
        public AsyncOperation UnloadSceneAsync(string sceneName)
        {
            AsyncOperation handle = SceneManager.UnloadSceneAsync(sceneName);
            return handle;
        }

        [Button(ButtonSizes.Medium, ButtonStyle.Box)]
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