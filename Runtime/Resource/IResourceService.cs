using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhikozzLib
{
    public interface IResourceService
    {
        Task<T> LoadAsync<T>(string key);
        Task<IList<T>> LoadAllAsync<T>(string label);
        void Release(string key);
        void ReleaseAll(string label);
    }
}