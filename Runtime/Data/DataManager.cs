using System.Collections.Generic;
using PhikozzLib;
using UnityEngine;

namespace PhikozzLib
{
    public class DataManager : MonoBehaviour, IServiceRegister
    {
        public DataContainer<TestData> TestData { get; private set; }

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
            InitTestData();
        }


        // BGData를 추가한 후, 아래와 같이 초기화 메서드를 작성할 수 있습니다.
        // 초기화 메서드를 작성한 후, Load() 메서드에서 호출해주세요.
        private void InitTestData()
        {
            var list = new List<TestData>();
            
            BG_TestData.ForEachEntity(data =>
            {
                list.Add(new TestData(data));
            });
            
            TestData = new DataContainer<TestData>(list);
        }
    }
}
