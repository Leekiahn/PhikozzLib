namespace PhikozzLib
{
    public interface IFloatingTextService
    {
        void RegisterFloatingText(BaseFloatingTextLoader loader);
        void UnRegisterFloatingText(string key);
        void Spawn(string key, string value, UnityEngine.Vector3 position, UnityEngine.Vector3 direction);
    }
}