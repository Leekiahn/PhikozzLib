using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PhikozzLib
{
    public class UIPointerFeedback : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
    {
        [Title("Pointer Feedbacks Settings")]
        [SerializeField] private bool _usePointerUpFeedback;
        [ShowIf("_usePointerUpFeedback")]
        [SerializeField] private MMF_Player _pointerUpFeedback;
        [PropertySpace(10)]

        [SerializeField] private bool _usePointerDownFeedback;
        [ShowIf("_usePointerDownFeedback")]
        [SerializeField] private MMF_Player _pointerDownFeedback;
        [PropertySpace(10)]

        [SerializeField] private bool _usePointerClickFeedback;
        [ShowIf("_usePointerClickFeedback")]
        [SerializeField] private MMF_Player _pointerClickFeedback;
        [PropertySpace(10)]

        [SerializeField] private bool _usePointerExitFeedback;
        [ShowIf("_usePointerExitFeedback")]
        [SerializeField] private MMF_Player _pointerExitFeedback;
        [PropertySpace(10)]

        [SerializeField] private bool _usePointerEnterFeedback;
        [ShowIf("_usePointerEnterFeedback")]
        [SerializeField] private MMF_Player _pointerEnterFeedback;
        [PropertySpace(10)]

        private void OnDisable()
        {
            StopAllFeedbacks();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_usePointerUpFeedback && _pointerUpFeedback != null)
            {
                _pointerUpFeedback.PlayFeedbacks();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_usePointerDownFeedback && _pointerDownFeedback != null)
            {
                _pointerDownFeedback.PlayFeedbacks();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_usePointerClickFeedback && _pointerClickFeedback != null)
            {
                _pointerClickFeedback.PlayFeedbacks();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_usePointerExitFeedback && _pointerExitFeedback != null)
            {
                _pointerExitFeedback.PlayFeedbacks();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_usePointerEnterFeedback && _pointerEnterFeedback != null)
            {
                _pointerEnterFeedback.PlayFeedbacks();
            }
        }

        private void StopAllFeedbacks()
        {
            if (_pointerUpFeedback != null)
            {
                _pointerUpFeedback.StopFeedbacks();
            }
            if (_pointerDownFeedback != null)
            {
                _pointerDownFeedback.StopFeedbacks();
            }
            if (_pointerClickFeedback != null)
            {
                _pointerClickFeedback.StopFeedbacks();
            }
            if (_pointerExitFeedback != null)
            {
                _pointerExitFeedback.StopFeedbacks();
            }
            if (_pointerEnterFeedback != null)
            {
                _pointerEnterFeedback.StopFeedbacks();
            }
        }
    }
}
