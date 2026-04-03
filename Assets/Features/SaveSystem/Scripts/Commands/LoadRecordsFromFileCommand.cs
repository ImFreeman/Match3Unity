using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace Assets.Features.SaveSystem.Scripts.Commands
{
    public class LoadRecordsFromFileCommand : ICommand
    {
        private string _filePath;
        private CancellationTokenSource _tokenSource = new CancellationTokenSource();
        public LoadRecordsFromFileCommand(string filePath)
        {
            _filePath = filePath;
        }

        public void Cancel()
        {
            _tokenSource.Cancel();
        }

        public void Dispose()
        {
            _tokenSource.Dispose();
            _tokenSource = null;
        }

        public async UniTask<CommandResult> Do()
        {
            try
            {
                var loadedData = await CsvDataLoader.LoadFromStreamingAssetsAsync(_filePath, _tokenSource.Token);
                if(_tokenSource.IsCancellationRequested)
                {
                    return new CommandResult() { Body = null, Status = CommandStatus.Canceled };
                }
                return new CommandResult() { Body = loadedData, Status = CommandStatus.Success };                
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                return new CommandResult() { Body = null, Status = CommandStatus.Failed };
            }            
        }
    }
}
