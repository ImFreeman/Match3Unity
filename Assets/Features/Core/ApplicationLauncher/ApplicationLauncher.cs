using Assets.Features.Core.Bootstrapper.Realization;
using Assets.Features.Core.Command.Realization;
using Assets.Features.UI.Scripts.Commands;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Features.Core.ApplicationLauncher
{
    public class ApplicationLauncher : MonoBehaviour
    {
        [SerializeField] private RectTransform _activeUIContainer;
        [SerializeField] private string _windowsPoolName;

        private ICommandBootstrapper _commandBootstrapper;

        private void Start()
        {
            StartApplication();
        }
        private async UniTaskVoid StartApplication()
        {
            _commandBootstrapper = new CommandBootstrapper();

            _commandBootstrapper.AddCommand(new InitUIServiceCommand(_activeUIContainer, _windowsPoolName));
            _commandBootstrapper.AddCommand(new StartMainMenuSceneCommand());

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