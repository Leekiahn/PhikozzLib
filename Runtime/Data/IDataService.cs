using UnityEngine;

public interface IDataService
{
    DataContainer<TestData> Item { get; }
    void Load();
}
