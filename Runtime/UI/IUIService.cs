using Cysharp.Threading.Tasks;
using PhikozzLib;

public interface IUIService 
{
    UniTask PreLoad(string label);
    T Open<T>() where T : UIBase;
    void Close<T>() where T : UIBase;
    void CloseAll();
}
