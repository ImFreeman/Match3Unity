using Cysharp.Threading.Tasks;

namespace Assets.Features.Core.Command.Realization
{
    public class LeaveRecordsSceneCommand : ICommand
    {
        private readonly int _nextSceneId;

        public LeaveRecordsSceneCommand(int nextSceneId)
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
            var result = await(new ClearRecordsSceneCommand()).Do();
            if (result.Status == CommandStatus.Success)
            {
                await(new ChangeSceneCommand(_nextSceneId)).Do();

                return new CommandResult() { Body = result.Body, Status = CommandStatus.Success };
            }
            return new CommandResult() { Body = result.Body, Status = result.Status };
        }
    }
}
