namespace PhikozzLib
{
    public abstract class UIPopup : UIBase
    {
        public bool IsOpened { get; private set; }
        
        public void Open()
        {
            OnOpen();
            
            IsOpened = true;
        }

        public void Close()
        {
            OnClose();
            
            IsOpened = false;
        }

        protected virtual void OnOpen()
        {
            gameObject.SetActive(true);
        }

        protected virtual void OnClose()
        {
            gameObject.SetActive(false);
        }
        
    }
}