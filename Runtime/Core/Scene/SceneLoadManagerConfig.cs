using MoreMountains.Tools;
using UnityEngine;

namespace PhikozzLib
{
    [CreateAssetMenu(fileName = "SceneLoadManagerConfig", menuName = "PhikozzLib/SceneLoadManagerConfig", order = 20)]
    public class SceneLoadManagerConfig : ScriptableObject
    {
        [SerializeField] private MMAdditiveSceneLoadingManagerSettings _settings;
        
        public MMAdditiveSceneLoadingManagerSettings Settings => _settings;
    }
}