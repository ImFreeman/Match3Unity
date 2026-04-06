using Assets.Features.UI.MainWindow.Scripts;
using Cysharp.Threading.Tasks;

namespace Assets.Features.Core.Command.Realization
{
    public class ClearMainMenuSceneCommand : BaseClearSceneCommand
    {
        public override UniTask<CommandResult> Do()
        {
            DisposeService<IUIService>();
            DisposeService<MainMenuWindowPresentor>();

            return new UniTask<CommandResult>(new CommandResult() { Body = null, Status = CommandStatus.Success });
        }
    }
}
