using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PhikozzLib
{
    public abstract class UISlot<TData> : UIBase, IPointerClickHandler
    {
        protected TData Data { get; private set; }

        [SerializeField] private bool _useClickFeedback;

        [ShowIf("_useClickFeedback")]
        [SerializeField] private MMF_Player _clickFeedback;

        private void OnDisable()
        {
            if (_clickFeedback != null)
            {
                _clickFeedback.StopFeedbacks();
            }
        }


        public void SetData(TData data)
        {
            Data = data;
            Refresh();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_useClickFeedback && _clickFeedback != null)
            {
                _clickFeedback.PlayFeedbacks();
            }

            OnClick();
        }

        protected abstract void OnClick();
    }
}
