namespace PhikozzLib
{
    public interface IDataService
    {
        void Register<T>(DataContainer<T> container) where T : BaseData;
        DataContainer<T> GetContainer<T>() where T : BaseData;
    }
}
