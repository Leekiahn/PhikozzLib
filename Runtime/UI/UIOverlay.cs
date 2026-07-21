using Cysharp.Threading.Tasks;
using PhikozzLib;
using UnityEngine;
using TMPro;


public abstract class UIOverlay : UIBase
{
    public void Show()
    {
        Refresh();
        OnShow();
        IsVisible = true;
    }
    
    public void Hide()
    {
        OnHide();
        IsVisible = false;
    }
    
    protected virtual void OnShow()
    {
        gameObject.SetActive(true);
    }
    
    protected virtual void OnHide()
    {
        gameObject.SetActive(false);
    }
}
