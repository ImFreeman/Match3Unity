using Assets.Features.Core.Command.Realization;
using Assets.Features.Core.ServiceLocatorScript;
using Assets.Features.SaveSystem.Scripts.Commands;
using Assets.Features.UI.AboutWindow.Scripts;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Features.Core.ApplicationLauncher.Launchers
{
    public class AboutSceneLauncher : CompositeApplicationLauncher
    {
        [SerializeField] private string _prefsKey;
        [SerializeField] private string _url;
        [SerializeField] private int _mainMenuSceneId;
        protected override IEnumerable<ICommand> GetCommands()
        {
            var commands = new List<ICommand>(base.GetCommands())
            {
                new AboutSceneLaunchCommand(_url, _mainMenuSceneId)
            };
            return commands;
        }

        private void OnApplicationQuit()
        {
            (new SaveRecordsCommand(_prefsKey)).Do();
            (new ClearGameSceneCommand()).Do();
            ServiceLocator.Clear();
        }
    }

    public class AboutSceneLaunchCommand : ICommand
    {
        private readonly string _url;
        private readonly int _mainMenuSceneId;

        public AboutSceneLaunchCommand(string url, int mainMenuSceneId)
        {
            _url = url;
            _mainMenuSceneId = mainMenuSceneId;
        }

        public void Cancel()
        {
            
        }

        public void Dispose()
        {
            
        }

        public UniTask<CommandResult> Do()
        {
            ServiceLocator.Register(new AboutWindowPresentor(
                ServiceLocator.Get<IUIService>(),
                _url,
                _mainMenuSceneId
                ));

            ServiceLocator.Get<IUIService>().Show<UIAboutWindow>();

            return new UniTask<CommandResult> (new CommandResult() { Body = null, Status = CommandStatus.Success } );
        }
    }
}
