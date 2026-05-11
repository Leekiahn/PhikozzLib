using UnityEngine.SceneManagement;
using System;
using UnityEngine;

public interface ISceneService 
{
    bool IsLoading { get; }
    string CurrentSceneName { get; }
    
    public event Action<string, LoadSceneMode> OnSceneLoaded;
    public event Action<string> OnSceneUnloaded;
    public event Action<string, string> OnSceneChanged;
    
    AsyncOperation LoadAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single);
    AsyncOperation UnloadAsync(string sceneName);
    AsyncOperation ReloadAsync();
}
