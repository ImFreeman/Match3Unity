using UnityEngine;
using UnityEngine.UI;

namespace Assets.Features.UI.UI_Button.Scripts
{
    public class UnityUIButton : UIButton
    {
        [SerializeField] private Button _button;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            ProccedClick();
        }
    }
}