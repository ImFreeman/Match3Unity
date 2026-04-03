using Assets.Features.Core.ServiceLocatorScript;
using Assets.Features.SaveSystem.Scripts.Interfaces;
using Assets.Features.UI.RecordsWindow.Scripts;
using Assets.Features.UI.RecordsWindow.Scripts.Commands;
using Assets.Features.UI.UIGraphicElement.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine.UIElements;

namespace Assets.Features.Core.Command.Realization
{
    public class StartRecordsSceneCommand : ICommand
    {
        private RecordDataView _prefab;
        private readonly int _mainMenuSceneId;
        public StartRecordsSceneCommand(RecordDataView prefab, int mainMenuSceneId)
        {
            _prefab = prefab;
            _mainMenuSceneId = mainMenuSceneId;
        }


        public void Cancel()
        {

        }

        public void Dispose()
        {
        }

        public UniTask<CommandResult> Do()
        {
            ServiceLocator.Register<IUIGraphicElementsSpawner<RecordDataView, RecordDataViewProtocol>>(new RecordDataView.RecordDataViewePool(_prefab));

            ServiceLocator.Register(new RecordsWindowPresentor(
                ServiceLocator.Get<IRecordsTrack>(),
                ServiceLocator.Get<IUIService>(),
                ServiceLocator.Get<IUIGraphicElementsSpawner<RecordDataView, RecordDataViewProtocol>>(),
                _mainMenuSceneId
                ));

            ServiceLocator.Get<IUIService>().Show<UIRecordsWindow>();

            if(ServiceLocator.TryGet<SetRecordHighlightedCommand>(out var command))
            {
                command.Do();
                command.Dispose();
                ServiceLocator.Unregister<SetRecordHighlightedCommand>();
            }

            return new UniTask<CommandResult>(new CommandResult() { Body = null, Status = CommandStatus.Success });
        }
    }
}
