using Cysharp.Threading.Tasks;

namespace PhikozzLib
{
    public interface IServiceInit
    {
        UniTask InitAsync();
    }
}
