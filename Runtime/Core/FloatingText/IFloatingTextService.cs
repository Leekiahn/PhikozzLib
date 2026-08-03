namespace PhikozzLib
{
    public interface IFloatingTextService
    {
        void RegisterFloatingText(BaseFloatingTextLoader loader);
        void UnRegisterFloatingText(eFloatingTextType type);
        void Spawn(eFloatingTextType type, string value, UnityEngine.Vector3 position, UnityEngine.Vector3 direction);
    }
}