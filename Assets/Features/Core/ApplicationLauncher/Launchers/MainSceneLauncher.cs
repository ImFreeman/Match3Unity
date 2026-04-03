using Assets.Features.Core.Command.Realization;
using Assets.Features.Core.ServiceLocatorScript;
using Assets.Features.SaveSystem.Scripts.Commands;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Features.Core.ApplicationLauncher
{
    public class MainSceneLauncher : CompositeApplicationLauncher
    {
        [SerializeField] private string _recordsFilePath;
        [SerializeField] private string _prefsKey;
        protected override IEnumerable<ICommand> GetCommands()
        {            
            var commands = new List<ICommand>(base.GetCommands())
            {
                new InitRecordsSystemCommand(_recordsFilePath, _prefsKey),
                new StartMainMenuSceneCommand()
            };
            return commands;
        }

        private void OnApplicationQuit()
        {
            (new SaveRecordsCommand(_prefsKey)).Do();
            (new ClearMainMenuSceneCommand()).Do();
            ServiceLocator.Clear();
        }
    }
}
