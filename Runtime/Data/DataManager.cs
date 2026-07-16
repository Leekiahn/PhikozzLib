using PhikozzLib;
using UnityEngine;

public class DataManager : MonoBehaviour, IDataService, IServiceRegister
{
    public void RegisterService()
    {
        ServiceLocator.Register<IDataService>(this);
    }
}
