using PhikozzLib;
using UnityEngine;

public abstract class UIToast : UIPopup
{
    [SerializeField] private float _duration;


    public void SetDuration(float duration)
    {
        _duration = duration;
    }
    
    public override void Open()
    {
        base.Open();
    }

    public override void Close()
    {
        base.Close();
    }
    
    
}
