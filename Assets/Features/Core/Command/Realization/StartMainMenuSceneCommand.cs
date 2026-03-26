using Assets.Features.Core.ServiceLocatorScript;
using Assets.Features.UI.MainWindow.Scripts;
using Cysharp.Threading.Tasks;

namespace Assets.Features.Core.Command.Realization
{
    public class StartMainMenuSceneCommand : ICommand
    {        
        public void Cancel()
        {
            
        }

        public void Dispose()
        {
            
        }

        public UniTask<CommandResult> Do()
        {
            if(ServiceLocator.TryGet<IUIService>(out var service))
            {
                service.Show<UIMainMenuWindow>();
                return new UniTask<CommandResult>(new CommandResult() { Body = null, Status = CommandStatus.Success});
            }
            else
            {
                return new UniTask<CommandResult>(new CommandResult() { Body = null, Status = CommandStatus.Failed });
            }
        }
    }
}
