using Assets.Features.Core.ServiceLocatorScript;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Features.UI.Scripts.Commands
{
    public class InitUIServiceCommand : ICommand
    {
        private RectTransform _activeContainer;
        private string _windowsPoolName;

        public InitUIServiceCommand(
            RectTransform activeContainer,
            string windowsPoolName
            )
        {
            _activeContainer = activeContainer;
            _windowsPoolName = windowsPoolName;
        }
        public void Dispose()
        {
            _activeContainer = null;
        }
        public void Cancel()
        {
            //TODO: add cancel
        }        

        public async UniTask<CommandResult> Do()
        {            
            IUIService service = GetUIService();
            await service.Init(_windowsPoolName);
            ServiceLocator.Register(service);

            return new CommandResult() { Status = CommandStatus.Success, Body = service };
        }

        private IUIService GetUIService()
        {
            return new UIService(_activeContainer);
        }
    }
}
