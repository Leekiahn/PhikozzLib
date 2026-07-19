using System;
using BansheeGz.BGDatabase;
using UnityEngine;

public static class GameData 
{
    public static DataContainer<TestData> Test { get; private set; }
    
    public static void Load()
    {
        Test = new DataContainer<TestData>();
        DB_TestData.ForEachEntity(data => Test.Add(new TestData(data)));
    }
}
