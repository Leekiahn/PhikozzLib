using Cysharp.Threading.Tasks;
using PhikozzLib;

public interface IUIService 
{
    void RegisterHUD<T>(T uiHud) where T : UIHUD;
    void UnregisterHUD<T>(T uihud) where T : UIHUD;
    T ShowHUD<T>() where T : UIHUD;
    void HideHUD<T>() where T : UIHUD;
    void HideAll();
}
