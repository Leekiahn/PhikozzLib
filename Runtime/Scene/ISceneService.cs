using UnityEngine;

namespace PhikozzLib
{
    public interface ISceneService
    {
        void LoadScene(string sceneName);
        void LoadSceneWithLoading(string sceneName, string loadingSceneName);
        
        void LoadAdditiveScene(string sceneName);
        void LoadAdditiveSceneWithLoading(string sceneName, string loadingSceneName);
        
        AsyncOperation LoadSceneAsync(string sceneName);
        AsyncOperation LoadAdditiveSceneAsync(string sceneName);
        
        AsyncOperation PreloadSceneAsync(string sceneName);
        AsyncOperation GetPreloadedSceneHandle();
        void ActivatePreloadedScene();
        
        AsyncOperation UnloadSceneAsync(string sceneName);
        
        void SetHold(eSceneLoadingHoldMode holdMode, bool status);
    }
}
