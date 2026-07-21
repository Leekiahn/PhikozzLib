using PhikozzLib;
using UnityEngine;

namespace PhikozzLib
{
    public class DataManager : MonoBehaviour, IServiceRegister
    {
        //public DataContainer<TestData> Test { get; private set; }
        //public DataContainer<DialogData> DialogDataContainer { get; private set; }

        private void Awake()
        {
            Load();
        }

        public void RegisterService()
        {
            ServiceLocator.Register(this);
            Load();
        }

        public void Load()
        {
            // InitTest();
            // InitDialog();
        }


        // private void InitTest()
        // {
        //     var list = new List<TestData>();
        //     
        //     DB_TestData.ForEachEntity(data =>
        //     {
        //         list.Add(new TestData(data));
        //     });
        //     
        //     Test = new DataContainer<TestData>(list);
        // }

        // private void InitDialog()
        // {
        //     var list = new List<DialogData>();
        //     
        //     DB_Test2Data.ForEachEntity(data =>
        //     {
        //         list.Add(new DialogData(data));
        //     });
        //     
        //     DialogDataContainer = new DataContainer<DialogData>(list);
        // }
    }
}
