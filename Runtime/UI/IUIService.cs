using UnityEngine;

public interface IUIService 
{
    T Open<T>() where T : UIBase;
    void Close<T>() where T : UIBase;
}
