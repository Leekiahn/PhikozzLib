using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PhikozzLib
{
    public abstract class UISlot<TData> : UIBase, IPointerClickHandler
    {
        protected TData Data { get; private set; }

        public void SetData(TData data)
        {
            Data = data;
            Refresh();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick();
        }

        protected abstract void OnClick();
    }
}
