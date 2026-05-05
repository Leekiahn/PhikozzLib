using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhikozzLib
{
    public interface IResourceService
    {
        T Load<T>(string key);
        Task<T> LoadAsync<T>(string key);
        
        List<T> LoadAll<T>(string label);
        Task<List<T>> LoadAllAsync<T>(string label);
        
        void Release<T>(T asset);
    }
}