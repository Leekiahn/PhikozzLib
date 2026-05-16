using UnityEngine.SceneManagement;

namespace PhikozzLib
{
    public readonly struct SceneUnloadedEvent
    {
        public Scene Scene { get; }

        public SceneUnloadedEvent(Scene scene)
        {
            Scene = scene;
        }
    }
}