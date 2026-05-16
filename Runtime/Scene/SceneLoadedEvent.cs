using UnityEngine.SceneManagement;

namespace PhikozzLib
{
    public readonly struct SceneLoadedEvent
    {
        public Scene Scene { get; }
        public LoadSceneMode LoadMode { get; }

        public SceneLoadedEvent(Scene scene, LoadSceneMode loadMode)
        {
            Scene = scene;
            LoadMode = loadMode;
        }
    }
}