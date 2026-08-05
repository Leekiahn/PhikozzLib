// using System.Collections.Generic;
// using PhikozzLib;
// using UnityEngine;
//
// public static class ExampleDataLoader
// {
//     private static IDataService _dataService;
//     
//     [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
//     private static void LoadData()
//     { 
//          _dataService = ServiceLocator.Get<IDataService>();
//
//          InitTestData();
//     }
//
//     private static void InitTestData()
//     {
//         var list = new List<TestData>();
//         
//         BG_TestData.ForEachEntity(data =>
//         {
//             list.Add(new TestData(data));
//         });
//         
//         _dataService.Register<TestData>(new DataContainer<TestData>(list));
//     }
// }
