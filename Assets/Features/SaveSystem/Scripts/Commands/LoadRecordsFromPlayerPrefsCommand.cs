using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Assets.Features.SaveSystem.Scripts.Commands
{
    internal class LoadRecordsFromPlayerPrefsCommand : ICommand
    {
        private readonly string _key;
        private CancellationTokenSource _tokenSource = new CancellationTokenSource();
        public LoadRecordsFromPlayerPrefsCommand(string filePath)
        {
            _key = filePath;
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

        public UniTask<CommandResult> Do()
        {
            try
            {
                if(!PlayerPrefs.HasKey(_key))
                {
                    return new UniTask<CommandResult>(new CommandResult() { Body = null, Status = CommandStatus.Success });
                }

                var jsonString = PlayerPrefs.GetString(_key);
                var loadedData = JsonConvert.DeserializeObject<IEnumerable<RecordData>>(jsonString);

                if (_tokenSource.IsCancellationRequested)
                {
                    return new UniTask<CommandResult>( new CommandResult() { Body = null, Status = CommandStatus.Canceled });
                }
                return new UniTask<CommandResult>( new CommandResult() { Body = loadedData, Status = CommandStatus.Success });
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                return new UniTask<CommandResult>( new CommandResult() { Body = null, Status = CommandStatus.Failed });
            }
        }
    }
}
