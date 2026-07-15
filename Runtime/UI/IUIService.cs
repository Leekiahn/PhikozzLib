using Cysharp.Threading.Tasks;
using PhikozzLib;

public interface IUIService 
{
    UniTask PreLoad();
    T OpenPopup<T>() where T : UIPopup;
    void ClosePopup<T>() where T : UIPopup;
    T ShowDialog<T>(string text, float typingDuration) where T : UIDialog;
    void HideDialog<T>() where T : UIDialog;
    void CloseAll();
}
