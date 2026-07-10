using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PhikozzLib
{
    public interface IAudioService
    {
        UniTask Load(string label, string key);
    }
}
