using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PhikozzLib
{
    public class UIDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {

        private static UIDragHandler _draggedHandler;

        private Canvas _rootCanvas;
        private Graphic[] _graphics;
        private bool[] _originalRaycastTargets;
        private RectTransform _rectTransform;

        private Transform _originalParent;
        private int _originalSiblingIndex;
        private Vector2 _originalAnchoredPosition;

        public event Action<UIDragHandler, UIDragHandler> OnSlotDropped;

        private void Awake()
        {
            _rootCanvas = GetComponentInParent<Canvas>().rootCanvas;
            _graphics = GetComponentsInChildren<Graphic>();
            _originalRaycastTargets = new bool[_graphics.Length];
            _rectTransform = (RectTransform)transform;
        }

        private void OnDisable()
        {
            if (_draggedHandler == this)
            {
                RestoreTransform();
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _draggedHandler = this;

            _originalParent = _rectTransform.parent;
            _originalSiblingIndex = _rectTransform.GetSiblingIndex();
            _originalAnchoredPosition = _rectTransform.anchoredPosition;

            _rectTransform.SetParent(_rootCanvas.transform, true);
            _rectTransform.SetAsLastSibling();

            for (int i = 0; i < _graphics.Length; i++)
            {
                _originalRaycastTargets[i] = _graphics[i].raycastTarget;
                _graphics[i].raycastTarget = false;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            RestoreTransform();
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (_draggedHandler != this)
            {
                var fromSwap = _draggedHandler.GetComponent<IUISlotDataSwap>();
                var toSwap = GetComponent<IUISlotDataSwap>();

                fromSwap?.SwapDataWith(toSwap);
                _draggedHandler.OnSlotDropped?.Invoke(_draggedHandler, this);
            }
        }

        private void RestoreTransform()
        {
            _rectTransform.SetParent(_originalParent, false);
            _rectTransform.SetSiblingIndex(_originalSiblingIndex);
            _rectTransform.anchoredPosition = _originalAnchoredPosition;

            for (int i = 0; i < _graphics.Length; i++)
            {
                _graphics[i].raycastTarget = _originalRaycastTargets[i];
            }

            _draggedHandler = null;
        }
    }
}
