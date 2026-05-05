using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhikozzLib
{
    public interface IResourceService
    {
        T Load<T>(string key);
        Task<T> LoadAsync<T>(string key);
        
        IList<T> LoadAll<T>(string label);
        Task<IList<T>> LoadAllAsync<T>(string label);
        
        void Release(string key);
        void ReleaseAll(string label);
    }
}