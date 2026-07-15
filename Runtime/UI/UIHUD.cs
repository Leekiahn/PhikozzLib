using UnityEngine;
using PhikozzLib;

public abstract class UIHUD : UIBase
{
    public bool IsVisible { get; private set; }

    public void Show()
    {
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
