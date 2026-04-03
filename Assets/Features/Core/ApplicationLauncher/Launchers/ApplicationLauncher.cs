using Assets.Features.Core.Bootstrapper.Realization;
using Assets.Features.Core.Command.Realization;
using Assets.Features.Core.ServiceLocatorScript;
using Assets.Features.UI.Scripts.Commands;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Features.Core.ApplicationLauncher
{
    public abstract class ApplicationLauncher : MonoBehaviour
    {
        

        private ICommandBootstrapper _commandBootstrapper;

        private void Start()
        {
            Debug.Log($"start {GetInstanceID()}");
            StartApplication();
        }

        protected abstract IEnumerable<ICommand> GetCommands();

        private async UniTaskVoid StartApplication()
        {
            _commandBootstrapper = new CommandBootstrapper();

            var commands = GetCommands();
            foreach (var command in commands)
            {
                _commandBootstrapper.AddCommand(command);
            }            

            var token = this.GetCancellationTokenOnDestroy();

            await _commandBootstrapper.ExecuteAsync(token);
        }

        private void OnDestroy()
        {
            _commandBootstrapper.Cancel();
            _commandBootstrapper.Dispose();
            _commandBootstrapper = null;
        }
    }
}