using UnityEngine;

namespace PhikozzLib
{
    public abstract class UIPopup : UIBase
    {
        public bool IsOpened { get; private set; }
        [SerializeField] private bool _hideOnAwake = true;
        [SerializeField] private bool _bringToFrontOnAwake = true;

        protected virtual void Awake()
        {
            if (_hideOnAwake)
            {
                gameObject.SetActive(false);
                IsOpened = false;
            }
            
            IsOpened = gameObject.activeSelf;
        }
        
        [ContextMenu("Open")]
        public virtual void Open()
        {
            OnOpen();
            IsOpened = true;
        }

        [ContextMenu("Close")]
        public virtual void Close()
        {
            OnClose();
            IsOpened = false;
        }

        protected virtual void OnOpen()
        {
            if (_bringToFrontOnAwake)
            {
                transform.SetAsLastSibling();
            }
            
            gameObject.SetActive(true);
        }

        protected virtual void OnClose()
        {
            gameObject.SetActive(false);
        }
        
    }
}