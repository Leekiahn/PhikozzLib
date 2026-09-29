using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace PhikozzLib
{
    public class PointerFeedback : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
    {
        [Title("Left Click Feedbacks Settings")]
        [SerializeField] private bool _useLeftPointerFeedback;
        [ShowIf("_useLeftPointerFeedback")]
        [SerializeField] private MMF_Player _leftPointerUpFeedback;
        [PropertySpace(10)]

        [ShowIf("_useLeftPointerFeedback")]
        [SerializeField] private MMF_Player _leftPointerDownFeedback;
        [PropertySpace(10)]

        [ShowIf("_useLeftPointerFeedback")]
        [SerializeField] private MMF_Player _leftPointerClickFeedback;
        [PropertySpace(10)]

        [Title("Right Click Feedbacks Settings")]
        [SerializeField] private bool _useRightPointerFeedback;
        [ShowIf("_useRightPointerFeedback")]
        [SerializeField] private MMF_Player _rightPointerUpFeedback;
        [PropertySpace(10)]

        [ShowIf("_useRightPointerFeedback")]
        [SerializeField] private MMF_Player _rightPointerDownFeedback;
        [PropertySpace(10)]

        [ShowIf("_useRightPointerFeedback")]
        [SerializeField] private MMF_Player _rightPointerClickFeedback;
        [PropertySpace(10)]

        [Title("Hover Feedbacks Settings")]
        [FormerlySerializedAs("_leftPointerEnterFeedback")]
        [SerializeField] private MMF_Player _pointerEnterFeedback;
        [PropertySpace(10)]

        [FormerlySerializedAs("_leftPointerExitFeedback")]
        [SerializeField] private MMF_Player _pointerExitFeedback;
        [PropertySpace(10)]

        [Title("Disabled Feedbacks Settings")]
        [SerializeField] private Selectable _selectable;
        [SerializeField] private MMF_Player _disabledClickFeedback;
        [PropertySpace(10)]

        private bool _isInteractable = true;

        private bool IsDisabled => !_isInteractable || (_selectable != null && !_selectable.IsInteractable());

        private void OnDisable()
        {
            StopAllFeedbacks();
        }

        public void SetInteractable(bool isInteractable)
        {
            _isInteractable = isInteractable;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            PlayButtonFeedback(eventData, _leftPointerUpFeedback, _rightPointerUpFeedback);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            PlayButtonFeedback(eventData, _leftPointerDownFeedback, _rightPointerDownFeedback);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (IsDisabled)
            {
                _disabledClickFeedback?.PlayFeedbacks();
                return;
            }

            PlayButtonFeedback(eventData, _leftPointerClickFeedback, _rightPointerClickFeedback);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PlayHoverFeedback(_pointerExitFeedback);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PlayHoverFeedback(_pointerEnterFeedback);
        }

        private void PlayButtonFeedback(PointerEventData eventData, MMF_Player leftFeedback, MMF_Player rightFeedback)
        {
            if (IsDisabled)
            {
                return;
            }

            if (eventData.button == PointerEventData.InputButton.Left)
            {
                leftFeedback?.PlayFeedbacks();
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                rightFeedback?.PlayFeedbacks();
            }
        }

        private void PlayHoverFeedback(MMF_Player feedback)
        {
            if (IsDisabled)
            {
                return;
            }

            feedback?.PlayFeedbacks();
        }

        private void StopAllFeedbacks()
        {
            if (_useLeftPointerFeedback)
            {
                _leftPointerUpFeedback?.StopFeedbacks();
                _leftPointerDownFeedback?.StopFeedbacks();
                _leftPointerClickFeedback?.StopFeedbacks();
            }

            if (_useRightPointerFeedback)
            {
                _rightPointerUpFeedback?.StopFeedbacks();
                _rightPointerDownFeedback?.StopFeedbacks();
                _rightPointerClickFeedback?.StopFeedbacks();
            }

            _pointerEnterFeedback?.StopFeedbacks();
            _pointerExitFeedback?.StopFeedbacks();
            _disabledClickFeedback?.StopFeedbacks();
        }


    }
}
