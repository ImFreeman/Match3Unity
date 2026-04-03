using Assets.Features.Core.ServiceLocatorScript;
using Assets.Features.TilesLayout.Scripts.Interfaces;
using Assets.Features.TilesLayout.Scripts.Realization;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Features.Core.Command.Realization
{
    public class StartGameSceneCommand : ICommand
    {
        public void Cancel()
        {
            
        }

        public void Dispose()
        {
            
        }

        public async UniTask<CommandResult> Do()
        {
            var tileLayout = ServiceLocator.Get<ITileLayout>();
            var windowPresenter = ServiceLocator.Get<GameWindowPresentor>();
            var uiWindow = ServiceLocator.Get<IUIService>().Get<UIGameWindow>();
            uiWindow.TilesField.GenerateBackgroundTexture(
                new UnityEngine.Vector2Int(tileLayout.TilesLayout.Length, tileLayout.TilesLayout[0].Length),
                (int)windowPresenter.GetTileSize().x);

            
            windowPresenter.UpdateView();

            return new CommandResult { Body = null, Status = CommandStatus.Success };
        }
    }
}
