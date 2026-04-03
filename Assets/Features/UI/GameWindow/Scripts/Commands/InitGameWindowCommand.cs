using Assets.Features.Core.ServiceLocatorScript;
using Assets.Features.GameLogic.Scripts.Interfaces;
using Assets.Features.Tile.Scripts.Interfaces;
using Assets.Features.Tile.Scripts.Realization;
using Assets.Features.TilesLayout.Scripts.Interfaces;
using Assets.Features.UI.UIGraphicElement.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Assets.Features.Core.Command.Realization
{
    public class InitGameWindowCommand : ICommand
    {
        public void Cancel()
        {
            
        }

        public void Dispose()
        {
        }

        public async UniTask<CommandResult> Do()
        {
            try
            {
                var gameWindow = new GameWindowPresentor(
                    ServiceLocator.Get<ITileStorage<TileModel>>(),
                    ServiceLocator.Get<IUIGraphicElementsSpawner<UIGraphicElementImage, UIGraphicElementProtocol>>(),
                    ServiceLocator.Get<ITileLayout>(),
                    ServiceLocator.Get<ITileSprites>(),
                    ServiceLocator.Get<IUIService>(),
                    ServiceLocator.Get<IScoreHandler>(),
                    ServiceLocator.Get<IMovesHandler>(),
                    ServiceLocator.Get<IUIGraphicElementStorage<UIGraphicElementImage>>()
                    );

                ServiceLocator.Register(gameWindow);
                ServiceLocator.Get<IUIService>().Show<UIGameWindow>();

                return new CommandResult() { Body = gameWindow, Status = CommandStatus.Success };
            }
            catch (InvalidOperationException ex)
            {
                return new CommandResult() { Body = null, Status = CommandStatus.Failed };
            }
        }
    }
}
