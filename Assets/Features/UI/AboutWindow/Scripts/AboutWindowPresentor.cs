using Assets.Features.Core.Command.Realization;
using System;
using UnityEngine;

namespace Assets.Features.UI.AboutWindow.Scripts
{
    public class AboutWindowPresentor : IDisposable
    {
        private UIAboutWindow _aboutWindow;
        private readonly string _url;
        private readonly int _nextSceneId;

        private ICommand _leaveCommand;

        public AboutWindowPresentor(IUIService uiService, string url, int nextSceneId)
        {
            _url = url;
            _nextSceneId = nextSceneId;
            _aboutWindow = uiService.Get<UIAboutWindow>();
            _leaveCommand = new LeaveAboutSceneCommand(_nextSceneId);

            _aboutWindow.BackButtonPressed += OnBackButtonPressed;
            _aboutWindow.LinkButtonPressed += OnLinkButtonPressed;            
        }

        public void Dispose()
        {
            _leaveCommand.Dispose();
            _leaveCommand = null;
            _aboutWindow.BackButtonPressed -= OnBackButtonPressed;
            _aboutWindow.LinkButtonPressed -= OnLinkButtonPressed;
            _aboutWindow = null;
        }

        private void OnLinkButtonPressed(object sender, EventArgs e)
        {
            Application.OpenURL(_url);
        }

        private void OnBackButtonPressed(object sender, EventArgs e)
        {
            _leaveCommand.Do();
        }

        
    }
}
