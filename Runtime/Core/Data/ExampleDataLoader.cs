using UnityEngine;

public static class ExampleDataLoader
{
    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void LoadData()
    { 
        // IDataService dataService = ServiceLocator.Get<IDataService>();
        //
        // var list = new List<TestData>();
        //
        // BG_TestData.ForEachEntity(data =>
        // {
        //     list.Add(new TestData(data));
        // });
        // dataService.Register(new DataContainer<TestData>(list));

        // var list2 = new List<TestData2>();
        //
        // BG_TestData2.ForEachEntity(data =>
        // {
        //     list2.Add(new TestData2(data));
        // });
        //dataService.Register(new DataContainer<TestData2>(list2));
    }
}
