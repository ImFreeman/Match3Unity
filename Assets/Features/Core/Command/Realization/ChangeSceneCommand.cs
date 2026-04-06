using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine.SceneManagement;

namespace Assets.Features.Core.Command.Realization
{
    public class ChangeSceneCommand : ICommand
    {
        private readonly int _sceneId;
        private CancellationTokenSource _tokenSource = new CancellationTokenSource();
        public ChangeSceneCommand(int sceneId)
        {
            _sceneId = sceneId;
        }

        public void Cancel()
        {
            _tokenSource.Cancel();
        }

        public void Dispose()
        {
            _tokenSource.Cancel();
            _tokenSource = null;
        }

        public async UniTask<CommandResult> Do()
        {
            try
            {
                await SceneManager.LoadSceneAsync(_sceneId).ToUniTask(cancellationToken: _tokenSource.Token);
            }
            catch (Exception ex)
            {
                return new CommandResult() { Body = ex, Status = CommandStatus.Failed };
            }

            if(_tokenSource.IsCancellationRequested)
            {
                return new CommandResult() { Body = null, Status = CommandStatus.Canceled };
            }

            return new CommandResult() { Body = null, Status = CommandStatus.Success };            
        }
    }
}
