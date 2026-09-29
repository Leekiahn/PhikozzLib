using Cysharp.Threading.Tasks;

namespace PhikozzLib
{
    public interface ISaveService 
    {
        void Save<T>(string key, T data);
        UniTask SaveAsync<T>(string key, T data);
        eSaveLoadResult Load<T>(string key, out T data);
        bool Exists(string key);
        void Delete(string key);
        void DeleteFolder(string folder);
        void DeleteAll();
    }
}
