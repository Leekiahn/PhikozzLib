using UnityEngine.SceneManagement;
using System;
using UnityEngine;

public interface ISceneService 
{
    event Action<Scene, LoadSceneMode> SceneLoaded;
    event Action<Scene> SceneUnloaded;
    event Action<Scene, Scene> ActiveSceneChanged;
    
    bool IsLoading { get; }
    Scene ActiveScene { get; }
    string ActiveSceneName { get; }
    
    AsyncOperation LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single);
    AsyncOperation UnloadSceneAsync(string sceneName);
    AsyncOperation ReloadSceneAsync();
    
    bool IsSceneLoaded(string sceneName);
    bool SetActiveScene(string sceneName);
}
