using Assets.Features.Core.Command.Realization;
using Assets.Features.UI.Scripts.Commands;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Features.Core.ApplicationLauncher.Installers
{
    public class UIServiceInstaller : BaseInstaller
    {
        [SerializeField] private RectTransform _activeUIContainer;
        [SerializeField] private string _windowsPoolName;
        public override IEnumerable<ICommand> GetInstallCommands()
        {
            return new ICommand[]
            {
                new InitUIServiceCommand(_activeUIContainer, _windowsPoolName)
            };
        }
    }
}
