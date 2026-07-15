using Cysharp.Threading.Tasks;
using PhikozzLib;
using UnityEngine;
using TMPro;


public abstract class UIDialog : UIBase
{
    [SerializeField] protected TMP_Text _tmpText;
    
    private float _typingDuration = 1f;
    private string _currentDialogText;

    public bool IsOpened { get; private set; }


    public void ShowDialog(string text, float typingDuration)
    {
        _currentDialogText = text;
        _typingDuration = typingDuration;
        
        OnShow();
        IsOpened = true;
    }
    
    public void HideDialog()
    {
        OnHide();
        IsOpened = false;
    }
    
    protected virtual void OnShow()
    {
        TypeTextAsync(_currentDialogText).Forget();
    }
    
    protected virtual void OnHide()
    {
        gameObject.SetActive(false);
    }

    private async UniTask TypeTextAsync(string message)
    {
        _tmpText.text = message;
        _tmpText.maxVisibleCharacters = 0;

        int delayPerCharacterMs = Mathf.Max(
            1,
            Mathf.RoundToInt((_typingDuration / message.Length) * 1000f)
        );

        foreach (char _ in message)
        {
            _tmpText.maxVisibleCharacters++;
            await UniTask.Delay(delayPerCharacterMs, DelayType.DeltaTime);
        }
    }
}
