using Assets.Features.Core.ServiceLocatorScript;
using Assets.Features.GameLogic.Scripts.Interfaces;
using Assets.Features.GameLogic.Scripts.Realization;
using Assets.Features.SaveSystem.Scripts.Interfaces;
using Assets.Features.Tile.Scripts.Interfaces;
using Assets.Features.Tile.Scripts.Realization;
using Assets.Features.TilesLayout.Scripts.Interfaces;
using Assets.Features.UI.GameOverPopUp.Scripts;
using Assets.Features.UI.UIGraphicElement.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Features.Tile.Scripts.Commands
{
    public class InitGameLogicCommand : ICommand
    {
        private readonly Color _recordHighlightColor;
        private readonly int _mainMenuSceneId;
        private readonly int _recordSceneId;
        private readonly int _minMatchSize;
        private readonly int _defaultMovesCount;
        private readonly int _scorePerTile;
        private readonly int _columns;
        private readonly int _rows;
        private IEnumerable<Vector2Int> _searchMatrix;
        private IEnumerable<(Vector2Int, TileType)> _staticTiles;
        private IReadOnlyDictionary<int, int> _movesAdditionData;

        public InitGameLogicCommand(
            int scorePerTile,
            int columns,
            int rows,
            IEnumerable<Vector2Int> searchMatrix,
            IEnumerable<(Vector2Int, TileType)> staticTiles,
            IReadOnlyDictionary<int, int> movesAdditionData,
            int defaultMovesCount,
            int minMatchSize,
            int recordSceneId,
            Color recordHighlightColor,
            int mainMenuSceneId)
        {
            _scorePerTile = scorePerTile;
            _columns = columns;
            _rows = rows;
            _searchMatrix = searchMatrix;
            _staticTiles = staticTiles;
            _movesAdditionData = movesAdditionData;
            _defaultMovesCount = defaultMovesCount;
            _minMatchSize = minMatchSize;
            _recordSceneId = recordSceneId;
            _recordHighlightColor = recordHighlightColor;
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
            ServiceLocator.Register<IMatchResolver>(new MatchResolver(
                ServiceLocator.Get<ITileStorage<TileModel>>(),
                ServiceLocator.Get<ITileLayout>(),
                ServiceLocator.Get<IMovesHandler>(),
                _movesAdditionData
                ));

            ServiceLocator.Register<IMatchFinder>(new MatchFinder(
                ServiceLocator.Get<ITileStorage<TileModel>>(),
                _searchMatrix
                ));

            ServiceLocator.Register<ITileShifter>(new TileShifter(
                ServiceLocator.Get<ITileLayout>(),
                ServiceLocator.Get<IUIGraphicElementStorage<UIGraphicElementImage>>(),
                ServiceLocator.Get<GameWindowPresentor>()
                ));

            ServiceLocator.Register<ITileResolver>(new TileResolver(
                ServiceLocator.Get<IScoreHandler>(),
                ServiceLocator.Get<ITileLayout>(),
                ServiceLocator.Get<ITileStorage<TileModel>>(),
                ServiceLocator.Get<ITileSpawner<TileModel, TileType>>(),
                _scorePerTile
                ));

            ServiceLocator.Register<ITileGenerator>(new TileGenerator<TileModel, TileType>(
                ServiceLocator.Get<ITileStorage<TileModel>>(),
                ServiceLocator.Get<ITileSpawner<TileModel, TileType>>(),
                ServiceLocator.Get<ITileLayout>(),
                ServiceLocator.Get<ITileResolver>(),
                _staticTiles
                ));

            ServiceLocator.Register<IGameField>(new GameField(
                ServiceLocator.Get<IMatchFinder>(),
                ServiceLocator.Get<ITileGenerator>(),
                ServiceLocator.Get<ITileLayout>(),
                ServiceLocator.Get<ITileShifter>(),
                ServiceLocator.Get<IMatchResolver>()
                ));

            ServiceLocator.Register(new TileClickHandler(
                ServiceLocator.Get<IGameField>(),
                ServiceLocator.Get<GameWindowPresentor>()
                ));

            ServiceLocator.Register(new GameOverPopUpPresentor(
                ServiceLocator.Get<IUIService>(),
                ServiceLocator.Get<GameWindowPresentor>(),
                _mainMenuSceneId
                ));

            ServiceLocator.Register(new GameFlow(
                ServiceLocator.Get<IMovesHandler>(),
                ServiceLocator.Get<IGameField>(),
                ServiceLocator.Get<ITileLayout>(),
                ServiceLocator.Get<IRecordsTrack>(),
                ServiceLocator.Get<IScoreHandler>(),
                ServiceLocator.Get<IUIService>(),
                _minMatchSize,
                _recordSceneId,
                _recordHighlightColor
                ));

            ServiceLocator.Get<IMovesHandler>().MovesCount = _defaultMovesCount;
            ServiceLocator.Get<IScoreHandler>().Score = 0;

            ServiceLocator.Get<ITileGenerator>().GenerateTilesLayout(_columns, _rows, null);            

            return new UniTask<CommandResult>(new CommandResult() { Body = null, Status = CommandStatus.Success });
        }
    }
}
