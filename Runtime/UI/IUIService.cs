using Cysharp.Threading.Tasks;
using PhikozzLib;

public interface IUIService
{
    UniTask LoadWindowPrefabs(string label);
    UniTask LoadOverlayPrefabs(string label);
    
    T OpenWindow<T>() where T : UIWindow;
    void CloseWindow<T>() where T : UIWindow;
    void CloseWindow(UIWindow window);
    void CloseAllWindow();
    
    T OpenOverlay<T>() where T : UIOverlay;
    void CloseOverlay<T>() where T : UIOverlay;
    void CloseOverlay(UIOverlay overlay);
    void CloseAllOverlay();
}
