using Assets.Features.Core.ServiceLocatorScript;
using Assets.Features.UI.AboutWindow.Scripts;
using Cysharp.Threading.Tasks;
using System;

namespace Assets.Features.Core.Command.Realization
{
    public class LeaveAboutSceneCommand : ICommand
    {
        private readonly int _nextSceneId;

        public LeaveAboutSceneCommand(int nextSceneId)
        {
            _nextSceneId = nextSceneId;
        }

        public void Cancel()
        {

        }

        public void Dispose()
        {

        }

        public async UniTask<CommandResult> Do()
        {
            var result = await (new ClearAboutSceneCommand()).Do();
            if (result.Status == CommandStatus.Success)
            {
                await (new ChangeSceneCommand(_nextSceneId)).Do();

                return new CommandResult() { Body = result.Body, Status = CommandStatus.Success };
            }
            return new CommandResult() { Body = result.Body, Status = result.Status };
        }
    }

    public class ClearAboutSceneCommand : ICommand
    {
        public void Cancel()
        {
            
        }

        public void Dispose()
        {
            
        }

        public UniTask<CommandResult> Do()
        {
            DisposeService<AboutWindowPresentor>();
            DisposeService<IUIService>();

            return new UniTask<CommandResult>(new CommandResult { Body = null, Status = CommandStatus.Success });
        }

        private void DisposeService<T>() where T : class, IDisposable
        {
            if (ServiceLocator.TryGet<T>(out T service))
            {
                service.Dispose();
                ServiceLocator.Unregister<T>();
            }
        }
    }
}
