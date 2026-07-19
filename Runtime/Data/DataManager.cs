using PhikozzLib;
using UnityEngine;

public class DataManager : MonoBehaviour, IDataService, IServiceRegister
{
    public DataContainer<TestData> Item => GameData.Test;

    public void RegisterService()
    {
        ServiceLocator.Register<IDataService>(this);
        Load();
    }

    public void Load()
    {
        GameData.Load();
    }
}