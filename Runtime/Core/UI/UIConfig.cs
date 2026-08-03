using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "UIConfig", menuName = "PhikozzLib/UI/UIConfig")]
public class UIConfig : ScriptableObject
{
    [SerializeField] private AssetLabelReference _windowLabelReference;
    [SerializeField] private AssetLabelReference _overlayLabelReference;
    
    public AssetLabelReference WindowLabelReference => _windowLabelReference;
    public AssetLabelReference OverlayLabelReference => _overlayLabelReference;
}
