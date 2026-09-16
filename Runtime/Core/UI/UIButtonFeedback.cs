using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace PhikozzLib
{
    public class UIButtonFeedback : MonoBehaviour
    {
        [SerializeField] private MMF_Player _clickFeedback;
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(PlayClickFeedback);
        }

        private void OnDisable()
        {
            if (_clickFeedback != null)
            {
                _clickFeedback.StopFeedbacks();
            }
        }

        private void PlayClickFeedback()
        {
            _clickFeedback.PlayFeedbacks();
        }
    }
}
