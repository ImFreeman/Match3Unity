using Assets.Features.Core.ServiceLocatorScript;
using Assets.Features.GameLogic.Scripts.Interfaces;
using Assets.Features.GameLogic.Scripts.Realization;
using Assets.Features.Tile.Scripts.Interfaces;
using Assets.Features.Tile.Scripts.Realization;
using Assets.Features.TilesLayout.Scripts.Interfaces;
using Assets.Features.UI.GameOverPopUp.Scripts;
using Assets.Features.UI.PauseWindow.Scripts;
using Assets.Features.UI.UIGraphicElement.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using System;

namespace Assets.Features.Core.Command.Realization
{
    public class ClearGameSceneCommand : ICommand
    {
        public void Cancel()
        {
        }

        public void Dispose()
        {
        }

        public UniTask<CommandResult> Do()
        {
            DisposeService<GameFlow>();
            DisposeService<GameOverPopUpPresentor>();

            DisposeService<IMatchResolver>();
            DisposeService<IMatchFinder>();
            DisposeService<ITileShifter>();
            DisposeService<ITileResolver>();
            DisposeService<ITileGenerator>();
            DisposeService<IGameField>();
            DisposeService<TileClickHandler>();

            DisposeService<PauseWindowPresentor>();

            DisposeService<GameWindowPresentor>();
            
            DisposeService<ITileStorage<TileModel>>();
            DisposeService<IUIGraphicElementsSpawner<UIGraphicElementImage, UIGraphicElementProtocol>>();
            DisposeService<ITileSprites>();
            DisposeService<ITileLayout>();
            DisposeService<IScoreHandler>();
            DisposeService<IMovesHandler>();
            DisposeService<ITileSpawner<TileModel, TileType>>();
            DisposeService<IUIGraphicElementStorage<UIGraphicElementImage>>();

            DisposeService<IUIService>();

            return new UniTask<CommandResult>(new CommandResult() { Body = null, Status = CommandStatus.Success });

        }

        private void DisposeService<T>() where T : class, IDisposable
        {
            if (ServiceLocator.TryGet<T>(out T service))
            {
                service.Dispose();
                ServiceLocator.Unregister<T>();
            }
        }
    }
}
