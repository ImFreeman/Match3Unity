using Assets.Features.Core.Command.Realization;
using System;
using UnityEngine.SceneManagement;

namespace Assets.Features.UI.PauseWindow.Scripts
{
    public class PauseWindowPresentor : IDisposable
    {
        private IUIService _uiService;
        private UIPauseWindow _window;
        private GameWindowPresentor _gameWindowPresentor;
        private readonly int _mainMenuSceneIndex;

        public PauseWindowPresentor(
            IUIService uiService,
            GameWindowPresentor gameWindowPresentor,
            int mainMenuSceneIndex
            )
        {
            _uiService = uiService;
            _window = uiService.Get<UIPauseWindow>();
            _mainMenuSceneIndex = mainMenuSceneIndex;
            _gameWindowPresentor = gameWindowPresentor;

            _window.Shown += OnShown;
            _window.Hidden += OnHidden;
            _window.ContinueButtonClicked += OnContinueButtonClicked;
            _window.MainMenuButtonClicked += OnMainMenuButtonClicked;            
        }

        public void Dispose()
        {
            _window.Shown -= OnShown;
            _window.Hidden -= OnHidden;
            _window.ContinueButtonClicked -= OnContinueButtonClicked;
            _window.MainMenuButtonClicked -= OnMainMenuButtonClicked;

            _uiService = null;
            _window = null;
            _gameWindowPresentor = null;
        }
        private void OnShown(object sender, EventArgs e)
        {
            _gameWindowPresentor.SetInputEnabled(false);
        }
        private void OnHidden(object sender, EventArgs e)
        {
            _gameWindowPresentor.SetInputEnabled(true);
        }
        private async void OnMainMenuButtonClicked(object sender, EventArgs e)
        {
            await (new LeaveGameSceneCommand(_mainMenuSceneIndex)).Do();
        }

        private void OnContinueButtonClicked(object sender, EventArgs e)
        {
            _uiService.Hide<UIPauseWindow>();
        }
    }
}
