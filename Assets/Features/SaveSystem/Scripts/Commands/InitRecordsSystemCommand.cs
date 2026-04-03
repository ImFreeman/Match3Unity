using Assets.Features.Core.ServiceLocatorScript;
using Assets.Features.SaveSystem.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Assets.Features.SaveSystem.Scripts.Commands
{
    public class InitRecordsSystemCommand : ICommand
    {
        private readonly string _filePath;
        private readonly string _prefsKey;
        private CancellationTokenSource _tokenSource = new CancellationTokenSource();
        public InitRecordsSystemCommand(string filePath, string prefsKey)
        {
            _filePath = filePath;
            _prefsKey = prefsKey;
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
            if(ServiceLocator.TryGet<IRecordsTrack>(out var service))
            {
                return new CommandResult() { Body = null, Status = CommandStatus.Success };
            }

            ServiceLocator.Register<IRecordsTrack>(new RecordsTrack());

            try
            {
                var loadedDataFromFile = await CsvDataLoader.LoadFromStreamingAssetsAsync(_filePath, _tokenSource.Token);

                if (_tokenSource.IsCancellationRequested)
                {
                    return new CommandResult() { Body = null, Status = CommandStatus.Canceled };
                }

                var recordTrack = ServiceLocator.Get<IRecordsTrack>();
                foreach (var recordData in loadedDataFromFile)
                {
                    recordTrack.AddRecord(recordData);
                }

                return new CommandResult() { Body = null, Status = CommandStatus.Success };
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                return new CommandResult() { Body = null, Status = CommandStatus.Failed };
            }
        }
    }
}
