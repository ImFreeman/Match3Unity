using Assets.Features.Core.ApplicationLauncher.Installers;
using Assets.Features.Core.ServiceLocatorScript;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Features.UI.MainWindow.Scripts
{
    public class MainWindowInstaller : BaseInstaller
    {
        [SerializeField] private int _gameSceneId;
        [SerializeField] private int _recordsSceneId;
        [SerializeField] private int _aboutSceneId;
        public override IEnumerable<ICommand> GetInstallCommands()
        {
            return new ICommand[] { new InitMainWindowCommands(_gameSceneId, _recordsSceneId, _aboutSceneId) };
        }
    }

    public class InitMainWindowCommands : ICommand
    {
        private int _gameSceneId;
        private int _recordsSceneId;
        private int _aboutSceneId;

        public InitMainWindowCommands(int gameSceneId, int recordsSceneId, int aboutSceneId)
        {
            _gameSceneId = gameSceneId;
            _recordsSceneId = recordsSceneId;
            _aboutSceneId = aboutSceneId;
        }

        public void Cancel()
        {
            
        }

        public void Dispose()
        {
            
        }

        public async UniTask<CommandResult> Do()
        {
            if(ServiceLocator.TryGet<IUIService>(out var service))
            {
                var mainWindow = new MainMenuWindowPresentor(service, _gameSceneId, _recordsSceneId, _aboutSceneId);
                ServiceLocator.Register(mainWindow);
                return new CommandResult() { Body = mainWindow, Status = CommandStatus.Success };
            }

            return new CommandResult() { Body = null, Status = CommandStatus.Failed };
        }
    }
}