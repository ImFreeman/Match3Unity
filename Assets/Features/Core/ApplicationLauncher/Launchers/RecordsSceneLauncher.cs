using Assets.Features.Core.ApplicationLauncher;
using Assets.Features.Core.Command.Realization;
using Assets.Features.Core.ServiceLocatorScript;
using Assets.Features.SaveSystem.Scripts.Commands;
using Assets.Features.UI.RecordsWindow.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class RecordsSceneLauncher : CompositeApplicationLauncher
{
    [SerializeField] private string _prefsKey;
    [SerializeField] private RecordDataView _prefab;
    [SerializeField] private int _mainMenuSceneId;
    protected override IEnumerable<ICommand> GetCommands()
    {
        var commands = new List<ICommand>(base.GetCommands())
            {
                new StartRecordsSceneCommand(_prefab, _mainMenuSceneId)
            };
        return commands;
    }

    private void OnApplicationQuit()
    {
        (new SaveRecordsCommand(_prefsKey)).Do();
        (new ClearGameSceneCommand()).Do();
        ServiceLocator.Clear();
    }
}
