using MoreMountains.Tools;
using UnityEngine;

namespace PhikozzLib
{
    [CreateAssetMenu(fileName = "SceneLoadingSettingsConfig", menuName = "PhikozzLib/Scene/SceneLoadingSettingsConfig")]
    public class SceneLoadingSettingsConfig : ScriptableObject
    {
        [SerializeField] private MMAdditiveSceneLoadingManagerSettings _settings;
        
        public MMAdditiveSceneLoadingManagerSettings Settings => _settings;
    }
}