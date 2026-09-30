using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PhikozzLib
{
    // 키보드·게임패드 UI 조작(선택 이동, 확인 입력)에 Feedback을 붙인다.
    public class NavigationFeedback : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler
    {
        [Title("Selection Feedbacks Settings")]
        [SerializeField] private MMF_Player _selectFeedback;
        [PropertySpace(10)]

        [SerializeField] private MMF_Player _deselectFeedback;
        [PropertySpace(10)]

        [Title("Submit Feedbacks Settings")]
        [SerializeField] private MMF_Player _submitFeedback;
        [PropertySpace(10)]

        [Title("Disabled Feedbacks Settings")]
        [SerializeField] private Selectable _selectable;
        [SerializeField] private MMF_Player _disabledSubmitFeedback;
        [PropertySpace(10)]

        private bool IsDisabled => _selectable != null && !_selectable.IsInteractable();

        private void Awake()
        {
            if (_selectable == null)
            {
                _selectable = GetComponent<Selectable>();
            }
        }

        private void OnDisable()
        {
            StopAllFeedbacks();
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (eventData is PointerEventData || IsDisabled)
            {
                return;
            }

            _selectFeedback?.PlayFeedbacks();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            if (eventData is PointerEventData || IsDisabled)
            {
                return;
            }

            _deselectFeedback?.PlayFeedbacks();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            if (IsDisabled)
            {
                _disabledSubmitFeedback?.PlayFeedbacks();
                return;
            }

            _submitFeedback?.PlayFeedbacks();
        }

        private void StopAllFeedbacks()
        {
            _selectFeedback?.StopFeedbacks();
            _deselectFeedback?.StopFeedbacks();
            _submitFeedback?.StopFeedbacks();
            _disabledSubmitFeedback?.StopFeedbacks();
        }
    }
}
