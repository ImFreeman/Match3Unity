using System.Collections.Generic;
using UnityEngine;

namespace Assets.Features.Core.ApplicationLauncher.Installers
{
    public abstract class BaseInstaller : MonoBehaviour
    {
        public abstract IEnumerable<ICommand> GetInstallCommands();
    }
}
