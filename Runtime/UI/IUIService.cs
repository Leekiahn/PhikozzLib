using Cysharp.Threading.Tasks;
using PhikozzLib;

public interface IUIService 
{
    UniTask PreLoad();
    
    void RegisterHUD<T>(T uiHud) where T : UIHUD;
    void UnregisterHUD<T>() where T : UIHUD;
    T ShowHUD<T>() where T : UIHUD;
    void HideHUD<T>() where T : UIHUD;
    T OpenPopup<T>() where T : UIPopup;
    void ClosePopup<T>() where T : UIPopup;
    T ShowDialog<T>(string text, float typingDuration) where T : UIDialog;
    void HideDialog<T>() where T : UIDialog;
    void CloseAll();
}
