using Assets.Features.Core.ServiceLocatorScript;
using Assets.Features.UI.RecordsWindow.Scripts;
using Assets.Features.UI.UIGraphicElement.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Features.Core.Command.Realization
{
    public class ClearRecordsSceneCommand : ICommand
    {
        public void Cancel()
        {
            
        }

        public void Dispose()
        {
            
        }

        public UniTask<CommandResult> Do()
        {
            DisposeService<RecordsWindowPresentor>();
            DisposeService<IUIGraphicElementsSpawner<RecordDataView, RecordDataViewProtocol>>();
            DisposeService<IUIService>();

            return new UniTask<CommandResult>(new CommandResult() { Body = null, Status = CommandStatus.Success });
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
