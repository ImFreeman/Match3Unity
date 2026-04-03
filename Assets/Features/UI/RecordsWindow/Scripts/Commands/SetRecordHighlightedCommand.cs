using Assets.Features.Core.ServiceLocatorScript;
using Assets.Features.SaveSystem.Scripts;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Features.UI.RecordsWindow.Scripts.Commands
{
    internal class SetRecordHighlightedCommand : ICommand
    {
        private RecordData _recordData;
        private Color _color;

        public SetRecordHighlightedCommand(RecordData recordData, Color color)
        {
            _recordData = recordData;
            _color = color;
        }

        public void Cancel()
        {
            _recordData = null;
        }

        public void Dispose()
        {
            
        }

        public UniTask<CommandResult> Do()
        {
            var view = ServiceLocator.Get<RecordsWindowPresentor>().GetView(_recordData.Date);
            view.Graphic.color = _color;

            return new UniTask<CommandResult>(new CommandResult { Body = null, Status = CommandStatus.Success});
        }
    }
}
