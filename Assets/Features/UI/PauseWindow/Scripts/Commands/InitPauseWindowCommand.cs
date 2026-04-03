using Assets.Features.Core.ServiceLocatorScript;
using Cysharp.Threading.Tasks;
using System;

namespace Assets.Features.UI.PauseWindow.Scripts.Commands
{
    public class InitPauseWindowCommand : ICommand
    {
        private int _mainMenuSceneIndex;

        public InitPauseWindowCommand(int mainMenuSceneIndex)
        {
            _mainMenuSceneIndex = mainMenuSceneIndex;
        }

        public void Cancel()
        {
            
        }

        public void Dispose()
        {
            
        }

        public UniTask<CommandResult> Do()
        {
            ServiceLocator.Register(new PauseWindowPresentor(
                ServiceLocator.Get<IUIService>(),
                ServiceLocator.Get<GameWindowPresentor>(),
                _mainMenuSceneIndex
                ));

            return new UniTask<CommandResult>(new CommandResult() { Body = null, Status = CommandStatus.Success });
        }
    }
}
