using Cysharp.Threading.Tasks;

namespace PhikozzLib
{
    public interface IUIService
    {
        T OpenWindow<T>() where T : UIWindow;
        void CloseWindow<T>() where T : UIWindow;
        void CloseWindow(UIWindow window);
        void CloseAllWindow();
    
        T OpenOverlay<T>() where T : UIOverlay;
        void CloseOverlay<T>() where T : UIOverlay;
        void CloseOverlay(UIOverlay overlay);
        void CloseAllOverlay();
    }
}