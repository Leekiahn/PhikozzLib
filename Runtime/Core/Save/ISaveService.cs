using Cysharp.Threading.Tasks;

namespace PhikozzLib
{
    public interface ISaveService 
    {
        void Save<T>(string key, T data);
        UniTask SaveAsync<T>(string key, T data);
        bool TryLoad<T>(string key, out T data);
        void Delete(string key);
        void DeleteAll();
    }
}
