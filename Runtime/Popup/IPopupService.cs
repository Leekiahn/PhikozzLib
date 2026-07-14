using Cysharp.Threading.Tasks;
using PhikozzLib;

public interface IPopupService 
{
    UniTask PreLoad(string label);
    T Open<T>() where T : UIPopup;
    void Close<T>() where T : UIPopup;
    void CloseAll();
}
