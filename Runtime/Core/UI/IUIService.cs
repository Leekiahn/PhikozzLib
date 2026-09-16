using Cysharp.Threading.Tasks;

namespace PhikozzLib
{
    public interface IUIService
    {
        void RegisterPopup<T>(T prefab) where T : UIPopup;
        void UnregisterPopup<T>(T prefab) where T : UIPopup;

        T OpenPopup<T>() where T : UIPopup;
        void ClosePopup<T>() where T : UIPopup;
        void ClosePopup(UIPopup window);
        void CloseAllPopup();
    }
}