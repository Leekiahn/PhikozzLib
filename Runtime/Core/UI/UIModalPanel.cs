using UnityEngine;
using UnityEngine.EventSystems;

namespace PhikozzLib
{
    public class UIModalPanel : MonoBehaviour, IPointerClickHandler
    {
        private UIPopup _popup;

        public void SetPopup(UIPopup popup)
        {
            _popup = popup;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ServiceLocator.Get<IUIService>().ClosePopup(_popup);
        }
    }
}
