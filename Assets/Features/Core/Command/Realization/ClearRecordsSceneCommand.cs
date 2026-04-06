using Assets.Features.UI.RecordsWindow.Scripts;
using Assets.Features.UI.UIGraphicElement.Scripts.Interfaces;
using Cysharp.Threading.Tasks;

namespace Assets.Features.Core.Command.Realization
{
    public class ClearRecordsSceneCommand : BaseClearSceneCommand
    {
        public override UniTask<CommandResult> Do()
        {
            DisposeService<RecordsWindowPresentor>();
            DisposeService<IUIGraphicElementsSpawner<RecordDataView, RecordDataViewProtocol>>();
            DisposeService<IUIService>();

            return new UniTask<CommandResult>(new CommandResult() { Body = null, Status = CommandStatus.Success });
        }
    }
}
