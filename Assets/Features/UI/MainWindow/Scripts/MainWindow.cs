using Assets.Features.Core.ServiceLocatorScript;
using System;
using UnityEngine;

namespace Assets.Features.UI.MainWindow.Scripts
{
    public class MainWindow : IDisposable
    {
        private IUIService _service;
        private UIMainMenuWindow _menuWindow;
        public MainWindow()
        {
            _service = GetUIService();

            _menuWindow = _service.Get<UIMainMenuWindow>();

            _menuWindow.PlayButtonPressed += OnPlayButtonPressed;
            _menuWindow.RecordsButtonPressed += OnRecordsButtonPressed;
            _menuWindow.AboutButtonPressed += OnAboutButtonPressed;
            _menuWindow.ExitButtonPressed += OnExitButtonPressed;
        }

        public void Dispose()
        {
            _menuWindow.PlayButtonPressed -= OnPlayButtonPressed;
            _menuWindow.RecordsButtonPressed -= OnRecordsButtonPressed;
            _menuWindow.AboutButtonPressed -= OnAboutButtonPressed;
            _menuWindow.ExitButtonPressed -= OnExitButtonPressed;
            _menuWindow = null;

            _service = null;
        }

        private IUIService GetUIService()
        {
            return ServiceLocator.Get<IUIService>();
        }

        private void OnExitButtonPressed(object sender, EventArgs e)
        {
            Application.Quit();
        }

        private void OnAboutButtonPressed(object sender, EventArgs e)
        {
            //set about scene
        }

        private void OnRecordsButtonPressed(object sender, EventArgs e)
        {
            // set records scene
        }

        private void OnPlayButtonPressed(object sender, EventArgs e)
        {
            //play scene
        }                
    }
}
