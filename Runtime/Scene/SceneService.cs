using System;
using PhikozzLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PhikozzLib
{
    public class SceneService : MonoBehaviour, ISceneService, IServiceRegister
    {
        public bool IsLoading { get; private set; }
        public string CurrentSceneName => SceneManager.GetActiveScene().name;

        public event Action<string, LoadSceneMode> OnSceneLoaded;
        public event Action<string> OnSceneUnloaded;
        public event Action<string, string> OnSceneChanged;

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
            SceneManager.sceneUnloaded -= HandleSceneUnloaded;
            SceneManager.activeSceneChanged -= HandleActiveSceneChanged;
        }

        public void RegisterService()
        {
            ServiceLocator.Register<ISceneService>(this);

            SceneManager.sceneLoaded += HandleSceneLoaded;
            SceneManager.sceneUnloaded += HandleSceneUnloaded;
            SceneManager.activeSceneChanged += HandleActiveSceneChanged;
        }

        public AsyncOperation LoadAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            IsLoading = true;

            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, mode);
            if (operation != null)
            {
                operation.completed += _ => IsLoading = false;
            }
            else
            {
                IsLoading = false;
            }

            return operation;
        }

        public AsyncOperation UnloadAsync(string sceneName)
        {
            IsLoading = true;

            AsyncOperation operation = SceneManager.UnloadSceneAsync(sceneName);
            if (operation != null)
            {
                operation.completed += _ => IsLoading = false;
            }
            else
            {
                IsLoading = false;
            }

            return operation;
        }

        public AsyncOperation ReloadAsync()
        {
            IsLoading = true;
            AsyncOperation operation = SceneManager.LoadSceneAsync(CurrentSceneName);
            if (operation != null)
            {
                operation.completed += _ => IsLoading = false;
            }
            else
            {
                IsLoading = false;
            }

            return operation;
        }

        private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            OnSceneLoaded?.Invoke(scene.name, mode);
        }

        private void HandleSceneUnloaded(Scene scene)
        {
            OnSceneUnloaded?.Invoke(scene.name);
        }

        private void HandleActiveSceneChanged(Scene oldScene, Scene newScene)
        {
            OnSceneChanged?.Invoke(oldScene.name, newScene.name);
        }
    }
}