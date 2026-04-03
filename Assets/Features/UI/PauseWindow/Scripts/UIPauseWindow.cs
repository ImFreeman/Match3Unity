using Assets.Features.UI.Scripts.Realization;
using System;
using UnityEngine;

namespace Assets.Features.UI.PauseWindow.Scripts
{
    public class UIPauseWindow : UIWindow
    {
        public event EventHandler ContinueButtonClicked;
        public event EventHandler MainMenuButtonClicked;

        [SerializeField] private UIButton _continueButton;
        [SerializeField] private UIButton _mainMenuButton;
        protected override void OnShow()
        {
            _continueButton.ButtonClicked += OnContinueButtonClicked;
            _mainMenuButton.ButtonClicked += OnMainMenuButtonClicked;
        }
        protected override void OnHide()
        {
            _continueButton.ButtonClicked -= OnContinueButtonClicked;
            _mainMenuButton.ButtonClicked -= OnMainMenuButtonClicked;
        }

        private void OnMainMenuButtonClicked(object sender, EventArgs e)
        {
            MainMenuButtonClicked?.Invoke(this, e);
        }

        private void OnContinueButtonClicked(object sender, EventArgs e)
        {
            ContinueButtonClicked?.Invoke(this, e);
        }                
    }
}
