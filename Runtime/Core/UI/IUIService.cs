namespace PhikozzLib
{
    public interface IUIService
    {
        void RegisterPopup(UIPopup prefab);
        void UnregisterPopup(UIPopup prefab);
        T OpenPopup<T>() where T : UIPopup;
        void ClosePopup<T>() where T : UIPopup;
        void ClosePopup(UIPopup window);
        void CloseAllPopup();
    }
}
