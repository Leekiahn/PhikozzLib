using System.Collections.Generic;
using BansheeGz.BGDatabase;
using PhikozzLib;
using UnityEngine;
using System;

public class DataManager : MonoBehaviour, IServiceRegister
{
    public DataContainer<TestData> Test { get; private set; }
    
    public void RegisterService()
    {
        ServiceLocator.Register(this);
        Load();
    }

    public void Load()
    {
        InitTest();
    }


    private void InitTest()
    {
        var list = new List<TestData>();
        
        DB_TestData.ForEachEntity(data =>
        {
            list.Add(new TestData(data));
        });
        
        Test = new DataContainer<TestData>(list);
    }
}
