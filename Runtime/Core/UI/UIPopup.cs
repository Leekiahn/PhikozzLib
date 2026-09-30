using UnityEngine;
using Sirenix.OdinInspector;
using MoreMountains.Feedbacks;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PhikozzLib
{
    public abstract class UIPopup : UIBase
    {
        public bool IsVisible { get; protected set; }

        [Title("Modal")]
        [SerializeField] private UIModalPanel _modalPanel;
        [PropertySpace(SpaceBefore = 20f)]

        [Title("Feedback")]
        [SerializeField] private MMF_Player _openFeedback;
        [SerializeField] private MMF_Player _closeFeedback;
        [PropertySpace(SpaceBefore = 20f)]

        [Title("Navigation")]
        [SerializeField] private Selectable _firstSelected;

        private GameObject _previousSelected;

        public virtual void Init()
        {
            if (_modalPanel != null)
            {
                _modalPanel.SetPopup(this);
            }

            if (_closeFeedback != null)
            {
                _closeFeedback.Events.OnComplete.AddListener(() =>
                {
                    gameObject.SetActive(false);
                });
            }
        }

        public void Open()
        {
            if (_closeFeedback != null)
            {
                _closeFeedback.StopFeedbacks();
            }

            Refresh();
            OnOpen();

            if (_openFeedback != null)
            {
                _openFeedback.PlayFeedbacks();
            }

            SelectFirstSelectable();
            IsVisible = true;
        }

        public void Close()
        {
            if (_closeFeedback != null)
            {
                _closeFeedback.PlayFeedbacks();
            }
            else
            {
                OnClose();
            }

            RestorePreviousSelected();
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

        private void SelectFirstSelectable()
        {
            if (_firstSelected != null)
            {
                _previousSelected = EventSystem.current.currentSelectedGameObject;
                EventSystem.current.SetSelectedGameObject(_firstSelected.gameObject);
            }
        }

        private void RestorePreviousSelected()
        {
            if (_previousSelected != null && _previousSelected.activeInHierarchy)
            {
                EventSystem.current.SetSelectedGameObject(_previousSelected);
            }
        }

    }
}
