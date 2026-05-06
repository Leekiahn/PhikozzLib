using System;
using PhikozzLib;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneService : MonoBehaviour, ISceneService, IServiceRegister
{
    public event Action<Scene, LoadSceneMode> SceneLoaded;
    public event Action<Scene> SceneUnloaded;
    public event Action<Scene, Scene> ActiveSceneChanged;

    public bool IsLoading { get; private set; }
    public Scene ActiveScene => SceneManager.GetActiveScene();
    public string ActiveSceneName => ActiveScene.name;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    public void RegisterService()
    {
        ServiceLocator.Register<ISceneService>(this);
    }

    public AsyncOperation LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
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
    
    public AsyncOperation UnloadSceneAsync(string sceneName)
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

    public AsyncOperation ReloadSceneAsync()
    {
        IsLoading = true;
        AsyncOperation operation = SceneManager.LoadSceneAsync(ActiveSceneName, LoadSceneMode.Single);
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

    public bool IsSceneLoaded(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        return scene.isLoaded;
    }

    public bool SetActiveScene(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (scene.isLoaded)
        {
            return SceneManager.SetActiveScene(scene);
        }

        return false;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneLoaded?.Invoke(scene, mode);
    }

    private void OnSceneUnloaded(Scene scene)
    {
        SceneUnloaded?.Invoke(scene);
    }

    private void OnActiveSceneChanged(Scene previousScene, Scene nextScene)
    {
        ActiveSceneChanged?.Invoke(previousScene, nextScene);
    }
}
