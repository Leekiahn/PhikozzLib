using UnityEngine.SceneManagement;
using System;
using UnityEngine;

namespace PhikozzLib
{
    public interface ISceneService
    {
        bool IsLoading { get; }
        string CurrentSceneName { get; }

        event Action<string, LoadSceneMode> OnSceneLoaded;
        event Action<string> OnSceneUnloaded;
        event Action<string, string> OnSceneChanged;

        AsyncOperation LoadAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single);
        AsyncOperation UnloadAsync(string sceneName);
        AsyncOperation ReloadAsync();
    }
}
