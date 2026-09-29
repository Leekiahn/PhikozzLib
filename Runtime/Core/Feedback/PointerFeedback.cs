using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

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

        private void OnDisable()
        {
            StopAllFeedbacks();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _leftPointerUpFeedback?.PlayFeedbacks();
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                _rightPointerUpFeedback?.PlayFeedbacks();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _leftPointerDownFeedback?.PlayFeedbacks();
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                _rightPointerDownFeedback?.PlayFeedbacks();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _leftPointerClickFeedback?.PlayFeedbacks();
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                _rightPointerClickFeedback?.PlayFeedbacks();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _pointerExitFeedback?.PlayFeedbacks();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _pointerEnterFeedback?.PlayFeedbacks();
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
        }
    }
}
