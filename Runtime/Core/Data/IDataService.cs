namespace PhikozzLib
{
    public interface IDataService
    {
        void AddDataContainer<T>(DataContainer<T> container) where T : BaseData;
        DataContainer<T> GetDataContainer<T>() where T : BaseData;
    }
}
