using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PhikozzLib
{
    [CreateAssetMenu(fileName = "UIManagerConfig", menuName = "PhikozzLib/UIManagerConfig", order = 10)]
    public class UIManagerConfig : ScriptableObject
    {
        [SerializeField] private AssetLabelReference _windowLabelReference;
        [SerializeField] private AssetLabelReference _overlayLabelReference;

        public AssetLabelReference WindowLabelReference => _windowLabelReference;
        public AssetLabelReference OverlayLabelReference => _overlayLabelReference;
    }
}
