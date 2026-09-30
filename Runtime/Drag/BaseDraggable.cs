using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PhikozzLib
{
    public abstract class BaseDraggable<TTarget> : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
        where TTarget : Component
    {
        [Title("Drag Settings")]
        [SerializeField] private bool _isUI = true;
        [PropertySpace(10)]

        [ShowIf("_isUI")]
        [SerializeField] private CanvasGroup _canvasGroup;

        [ShowIf("_isUI")]
        [SerializeField] private Transform _dragParent;

        [HideIf("_isUI")]
        [SerializeField] private Collider2D _collider2D;

        [HideIf("_isUI")]
        [SerializeField] private Collider _collider;
        [PropertySpace(10)]

        [Title("Drag Feedbacks Settings")]
        [SerializeField] private MMF_Player _beginDragFeedback;
        [SerializeField] private MMF_Player _dropSuccessFeedback;
        [SerializeField] private MMF_Player _dropFailFeedback;
        [SerializeField] private MMF_Player _deniedDragFeedback;
        [PropertySpace(10)]

        private Transform _originParent;
        private int _originSiblingIndex;
        private Vector3 _originPosition;
        private Vector3 _grabOffset;
        private bool _isDragging;

        protected virtual void Awake()
        {
            if (_isUI && _canvasGroup == null)
            {
                _canvasGroup = GetComponent<CanvasGroup>();

                if (_canvasGroup == null)
                {
                    _canvasGroup = gameObject.AddComponent<CanvasGroup>();
                }
            }

            if (!_isUI)
            {
                if (_collider2D == null)
                {
                    _collider2D = GetComponent<Collider2D>();
                }

                if (_collider == null)
                {
                    _collider = GetComponent<Collider>();
                }
            }
        }

        protected virtual void OnDisable()
        {
            if (_isDragging)
            {
                SetBlocksRaycast(true);
                _isDragging = false;
            }

            StopAllFeedbacks();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!CanBeginDrag())
            {
                _deniedDragFeedback?.PlayFeedbacks();
                eventData.pointerDrag = null;
                return;
            }

            _isDragging = true;
            _originParent = transform.parent;
            _originSiblingIndex = transform.GetSiblingIndex();
            _originPosition = transform.position;
            _grabOffset = transform.position - GetPointerWorldPosition(eventData);

            if (_isUI && _dragParent != null)
            {
                transform.SetParent(_dragParent, true);
                transform.SetAsLastSibling();
            }

            SetBlocksRaycast(false);
            _beginDragFeedback?.PlayFeedbacks();
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = GetPointerWorldPosition(eventData) + _grabOffset;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _isDragging = false;
            SetBlocksRaycast(true);

            TTarget target = FindDropTarget(eventData);

            if (target != null && TryDrop(target))
            {
                _dropSuccessFeedback?.PlayFeedbacks();
                return;
            }

            ReturnToOrigin();
            _dropFailFeedback?.PlayFeedbacks();
        }

        protected abstract bool TryDrop(TTarget target);

        protected virtual bool CanBeginDrag()
        {
            return true;
        }

        protected virtual void ReturnToOrigin()
        {
            transform.SetParent(_originParent, true);
            transform.SetSiblingIndex(_originSiblingIndex);
            transform.position = _originPosition;
        }

        private TTarget FindDropTarget(PointerEventData eventData)
        {
            GameObject hitObject = eventData.pointerCurrentRaycast.gameObject;

            if (hitObject == null)
            {
                return null;
            }

            return hitObject.GetComponentInParent<TTarget>();
        }

        private Vector3 GetPointerWorldPosition(PointerEventData eventData)
        {
            if (_isUI)
            {
                var parentRect = (RectTransform)transform.parent;
                RectTransformUtility.ScreenPointToWorldPointInRectangle(parentRect, eventData.position, eventData.pressEventCamera, out Vector3 uiWorldPosition);
                return uiWorldPosition;
            }

            Camera eventCamera = eventData.pressEventCamera;
            float depth = eventCamera.WorldToScreenPoint(transform.position).z;
            return eventCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, depth));
        }

        private void SetBlocksRaycast(bool isBlocking)
        {
            if (_isUI)
            {
                _canvasGroup.blocksRaycasts = isBlocking;
                return;
            }

            if (_collider2D != null)
            {
                _collider2D.enabled = isBlocking;
            }

            if (_collider != null)
            {
                _collider.enabled = isBlocking;
            }
        }

        private void StopAllFeedbacks()
        {
            _beginDragFeedback?.StopFeedbacks();
            _dropSuccessFeedback?.StopFeedbacks();
            _dropFailFeedback?.StopFeedbacks();
            _deniedDragFeedback?.StopFeedbacks();
        }
    }
}
