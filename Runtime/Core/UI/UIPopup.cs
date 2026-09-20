using UnityEngine;
using Sirenix.OdinInspector;
using MoreMountains.Feedbacks;

namespace PhikozzLib
{
    public abstract class UIPopup : UIBase
    {
        public bool IsVisible { get; protected set; }

        [SerializeField] private bool _useFeedback;
        [ShowIf("_useFeedback")]
        [SerializeField] private MMF_Player _openFeedback;

        [ShowIf("_useFeedback")]
        [SerializeField] private MMF_Player _closeFeedback;

        public virtual void Init()
        {
            if (_useFeedback && _closeFeedback != null)
            {
                _closeFeedback.Events.OnComplete.AddListener(() =>
                {
                    gameObject.SetActive(false);
                });
            }
        }

        public void Open()
        {
            if (_useFeedback && _closeFeedback != null)
            {
                _closeFeedback.StopFeedbacks();
            }

            Refresh();
            OnOpen();

            if (_useFeedback && _openFeedback != null)
            {
                _openFeedback.PlayFeedbacks();
            }

            IsVisible = true;
        }

        public void Close()
        {
            if (_useFeedback && _closeFeedback != null)
            {
                _closeFeedback.PlayFeedbacks();
            }
            else
            {
                OnClose();
            }

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