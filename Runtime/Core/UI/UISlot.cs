using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PhikozzLib
{
    public abstract class UISlot<TData> : UIBase, IUIDragDataHandler, IPointerClickHandler
    {
        protected TData Data { get; private set; }

        public void SetData(TData data)
        {
            Data = data;
            Refresh();
        }

        public virtual void HandleDragDataWith(IUIDragDataHandler other)
        {
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnLeftClick();
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                OnRightClick();
            }
        }

        protected virtual void OnLeftClick() {}
        protected virtual void OnRightClick() {}
    }
}
