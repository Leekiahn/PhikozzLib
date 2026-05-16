using UnityEngine.SceneManagement;

namespace PhikozzLib
{
    public readonly struct ActiveSceneChangedEvent
    {
        public Scene OldScene { get; }
        public Scene NewScene { get; }

        public ActiveSceneChangedEvent(Scene oldScene, Scene newScene)
        {
            OldScene = oldScene;
            NewScene = newScene;
        }
    }
}