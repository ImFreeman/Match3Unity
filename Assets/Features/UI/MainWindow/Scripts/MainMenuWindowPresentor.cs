using Assets.Features.Core.Bootstrapper.Realization;
using Assets.Features.Core.Command.Realization;
using System;
using UnityEngine;

namespace Assets.Features.UI.MainWindow.Scripts
{
    public class MainMenuWindowPresentor : IDisposable
    {
        private IUIService _service;
        private UIMainMenuWindow _menuWindow;
        private readonly int _gameWindowId;
        private readonly int _recordsSceneId;
        private readonly int _aboutSceneId;

        private ICommandBootstrapper _goToGameSceneBootstrap;
        private ICommandBootstrapper _goToRecordsSceneBootstrap;
        private ICommandBootstrapper _goToAboutSceneBootstrap;

        public MainMenuWindowPresentor(
            IUIService service,
            int gameWindowId,
            int recordsSceneId,
            int aboutSceneId)
        {
            _service = service;

            _menuWindow = _service.Get<UIMainMenuWindow>();

            _menuWindow.PlayButtonPressed += OnPlayButtonPressed;
            _menuWindow.RecordsButtonPressed += OnRecordsButtonPressed;
            _menuWindow.AboutButtonPressed += OnAboutButtonPressed;
            _menuWindow.ExitButtonPressed += OnExitButtonPressed;
            _gameWindowId = gameWindowId;
            _recordsSceneId = recordsSceneId;
            _aboutSceneId = aboutSceneId;

            _goToGameSceneBootstrap = new CommandBootstrapper();
            _goToGameSceneBootstrap.AddCommand(new ClearMainMenuSceneCommand());
            _goToGameSceneBootstrap.AddCommand(new ChangeSceneCommand(_gameWindowId));

            _goToRecordsSceneBootstrap = new CommandBootstrapper();
            _goToRecordsSceneBootstrap.AddCommand(new ClearMainMenuSceneCommand());
            _goToRecordsSceneBootstrap.AddCommand(new ChangeSceneCommand(_recordsSceneId));

            _goToAboutSceneBootstrap = new CommandBootstrapper();
            _goToAboutSceneBootstrap.AddCommand(new ClearMainMenuSceneCommand());
            _goToAboutSceneBootstrap.AddCommand(new ChangeSceneCommand(_aboutSceneId));
            
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

        private void OnExitButtonPressed(object sender, EventArgs e)
        {
            Application.Quit();
        }

        private void OnAboutButtonPressed(object sender, EventArgs e)
        {
            _goToAboutSceneBootstrap.ExecuteAsync(default);
        }

        private void OnRecordsButtonPressed(object sender, EventArgs e)
        {
            _goToRecordsSceneBootstrap.ExecuteAsync(default);
        }

        private void OnPlayButtonPressed(object sender, EventArgs e)
        {            
            _goToGameSceneBootstrap.ExecuteAsync(default);
        }                
    }
}
