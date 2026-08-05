using UnityEngine;

namespace PhikozzLib
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UIWindow : UIBase
    {
        private CanvasGroup _canvasGroup;

        public virtual void Init()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        
        public void Open()
        {
            Refresh();
            OnOpen();
            IsVisible = true;
        }

        public void Close()
        {
            OnClose();
            IsVisible = false;
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