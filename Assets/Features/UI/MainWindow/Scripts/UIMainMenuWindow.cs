using Assets.Features.UI.Scripts.Realization;
using System;
using UnityEngine;

namespace Assets.Features.UI.MainWindow.Scripts
{
    public class UIMainMenuWindow : UIWindow
    {
        public event EventHandler PlayButtonPressed;
        public event EventHandler RecordsButtonPressed;
        public event EventHandler AboutButtonPressed;
        public event EventHandler ExitButtonPressed;

        [SerializeField] private UIButton _playButton;
        [SerializeField] private UIButton _recordsButton;
        [SerializeField] private UIButton _aboutButton;
        [SerializeField] private UIButton _exitButton;

        protected override void OnShow()
        {
            _playButton.ButtonClicked += OnPlayButtonPressed;
            _recordsButton.ButtonClicked += OnRecordsButtonPressed;
            _aboutButton.ButtonClicked += OnAboutButtonPressed;
            _exitButton.ButtonClicked += OnExitButtonPressed;
        }

        protected override void OnHide()
        {
            _playButton.ButtonClicked -= OnPlayButtonPressed;
            _recordsButton.ButtonClicked -= OnRecordsButtonPressed;
            _aboutButton.ButtonClicked -= OnAboutButtonPressed;
            _exitButton.ButtonClicked -= OnExitButtonPressed;
        }

        private void OnPlayButtonPressed(object sender, EventArgs args)
        {
            PlayButtonPressed?.Invoke(this, EventArgs.Empty);
        }
        private void OnRecordsButtonPressed(object sender, EventArgs args)
        {
            RecordsButtonPressed?.Invoke(this, EventArgs.Empty);
        }
        private void OnAboutButtonPressed(object sender, EventArgs args)
        {
            AboutButtonPressed?.Invoke(this, EventArgs.Empty);
        }
        private void OnExitButtonPressed(object sender, EventArgs args)
        {
            ExitButtonPressed?.Invoke(this, EventArgs.Empty);
        }        
    }
}