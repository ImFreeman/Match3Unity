using Assets.Features.Core.ApplicationLauncher.Installers;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Features.Core.ApplicationLauncher
{
    public class CompositeApplicationLauncher : ApplicationLauncher
    {
        [SerializeField] private BaseInstaller[] installers;
        protected override IEnumerable<ICommand> GetCommands()
        {
            Debug.Log($"get commands {GetInstanceID()}");
            var commands = new List<ICommand>() { Capacity = installers.Length };
            foreach (var installer in installers)
            {
                commands.AddRange(installer.GetInstallCommands());                
            }

            return commands;
        }
    }
}
