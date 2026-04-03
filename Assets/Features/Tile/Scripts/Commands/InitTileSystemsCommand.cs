using Assets.Features.Core.ServiceLocatorScript;
using Assets.Features.GameLogic.Scripts.Interfaces;
using Assets.Features.GameLogic.Scripts.Realization;
using Assets.Features.Tile.Scripts.Interfaces;
using Assets.Features.Tile.Scripts.Realization;
using Assets.Features.TilesLayout.Scripts.Interfaces;
using Assets.Features.TilesLayout.Scripts.Realization;
using Assets.Features.UI.UIGraphicElement.Scripts.Interfaces;
using Cysharp.Threading.Tasks;

namespace Assets.Features.Core.Command.Realization
{
    public class InitTileSystemsCommand : ICommand
    {
        private readonly int _initPoolCapacity;
        private UIGraphicElementImage _tilePrefab;
        private TileSpritesScriptableObject _tileSprites;

        public InitTileSystemsCommand(
            int initPoolCapacity,
            UIGraphicElementImage tilePrefab,
            TileSpritesScriptableObject tileSprites
            )
        {
            _initPoolCapacity = initPoolCapacity;
            _tilePrefab = tilePrefab;
            _tileSprites = tileSprites;
        }

        public void Cancel()
        {
            
        }

        public void Dispose()
        {
            _tilePrefab = null;
            _tileSprites = null;
        }

        public async UniTask<CommandResult> Do()
        {
            ServiceLocator.Register<ITileStorage<TileModel>>(new TileStorage<TileModel>());
            ServiceLocator.Register<IUIGraphicElementsSpawner<UIGraphicElementImage, UIGraphicElementProtocol>>(
                new UIGraphicElementImage.UIGraphicElementImagePool(_tilePrefab, _initPoolCapacity)
                );
            ServiceLocator.Register<ITileSprites>(new TileSpritesSOWrapper(_tileSprites));
            ServiceLocator.Register<ITileLayout>(new TileLayout());
            ServiceLocator.Register<IScoreHandler>(new ScoreHandler());
            ServiceLocator.Register<IMovesHandler>(new MovesHandler());          
            ServiceLocator.Register<ITileSpawner<TileModel, TileType>>(new TilePool(_initPoolCapacity));
            ServiceLocator.Register<IUIGraphicElementStorage<UIGraphicElementImage>>(new UIGraphicElementStorage());


            return new CommandResult() { Body = null, Status = CommandStatus.Success };
        }
    }
}
