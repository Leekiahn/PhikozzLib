using Cysharp.Threading.Tasks;

public interface IDialogService
{
    UniTask PreLoad();
    UniTask LoadDialogPrefabs(string label);
    T Show<T>(string text, float typingDuration) where T : UIDialog;
    void Hide<T>() where T : UIDialog;
    void CloseAll();
}
