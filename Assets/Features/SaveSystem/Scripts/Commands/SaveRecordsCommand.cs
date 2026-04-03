using Assets.Features.Core.ServiceLocatorScript;
using Assets.Features.SaveSystem.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Features.SaveSystem.Scripts.Commands
{
    public class SaveRecordsCommand : ICommand
    {
        private readonly string _key;

        public SaveRecordsCommand(string key)
        {
            _key = key;
        }

        public void Cancel()
        {
            
        }

        public void Dispose()
        {
            
        }

        public UniTask<CommandResult> Do()
        {
            var records = ServiceLocator.Get<IRecordsTrack>().GetAllRecods();
            var jsonString = JsonConvert.SerializeObject(records);

            PlayerPrefs.SetString(_key, jsonString);
            PlayerPrefs.Save();

            return new UniTask<CommandResult>(new CommandResult() { Body = null, Status = CommandStatus.Success});
        }
    }

    
}
