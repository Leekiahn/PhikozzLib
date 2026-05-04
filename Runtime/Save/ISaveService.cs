namespace PhikozzLib
{
    public interface ISaveService 
    {
        void Save<T>(string key, T data);
        T Load<T>(string key);
        void Delete(string key);
        void DeleteAll();
    }
}
