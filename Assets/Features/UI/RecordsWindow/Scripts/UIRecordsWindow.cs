using Assets.Features.UI.Scripts.Realization;
using System;
using UnityEngine;

namespace Assets.Features.UI.RecordsWindow.Scripts
{
    public class UIRecordsWindow : UIWindow
    {
        public event EventHandler MainMenuButtonClicked;

        public RectTransform ContentTransform => _contentTransform;

        [SerializeField] private UIButton _mainMenuButton;
        [SerializeField] private RectTransform _contentTransform;

        protected override void OnShow()
        {
            _mainMenuButton.ButtonClicked += OnButtonClicked;
        }

        protected override void OnHide()
        {
            _mainMenuButton.ButtonClicked -= OnButtonClicked;
        }        

        private void OnButtonClicked(object sender, System.EventArgs e)
        {
            MainMenuButtonClicked?.Invoke(this, e);
        }
    }
}