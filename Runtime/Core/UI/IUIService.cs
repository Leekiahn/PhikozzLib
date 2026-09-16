using Cysharp.Threading.Tasks;

namespace PhikozzLib
{
    public interface IUIService
    {
        T OpenPopup<T>() where T : UIPopup;
        void ClosePopup<T>() where T : UIPopup;
        void ClosePopup(UIPopup window);
        void CloseAllPopup();
    }
}