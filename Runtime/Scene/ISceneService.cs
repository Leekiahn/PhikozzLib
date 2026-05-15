using UnityEngine.SceneManagement;
using System;
using UnityEngine;

namespace PhikozzLib
{
    public interface ISceneService
    {
        bool IsLoading { get; }
        string CurrentSceneName { get; }

        AsyncOperation LoadAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single);
        AsyncOperation UnloadAsync(string sceneName);
        AsyncOperation ReloadAsync();
    }
}
