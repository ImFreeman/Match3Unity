using Assets.Features.Core.Command.Realization;
using System;

namespace Assets.Features.UI.GameOverPopUp.Scripts
{
    public class GameOverPopUpPresentor : IDisposable
    {
        private GameWindowPresentor _gameWindowPresentor;
        private UIGameOverPopUp _window;
        private IUIService _uiService;
        private readonly int _nextSceneId;

        public GameOverPopUpPresentor(IUIService uiService, GameWindowPresentor gameWindowPresentor, int nextSceneId)
        {
            _uiService = uiService;
            _nextSceneId = nextSceneId;
            _gameWindowPresentor = gameWindowPresentor;
            _window = uiService.Get<UIGameOverPopUp>();

            _window.Shown += OnShown;
            _window.Hidden += OnHidden;
            _window.OkButtonPressed += OnOkButtonPressed;
        }

        private void OnShown(object sender, EventArgs e)
        {
            _gameWindowPresentor.SetInputEnabled(false);            
        }

        private async void OnOkButtonPressed(object sender, EventArgs e)
        {
            _uiService.Hide<UIGameOverPopUp>();
            await (new LeaveGameSceneCommand(_nextSceneId)).Do();
        }

        private void OnHidden(object sender, EventArgs e)
        {
            _gameWindowPresentor.SetInputEnabled(true);
            _window.OkButtonPressed -= OnOkButtonPressed;
        }
        
        public void Dispose()
        {
            _window.OkButtonPressed -= OnOkButtonPressed;
            _window.Shown -= OnShown;
            _window.Hidden -= OnHidden;
        }
    }
}
