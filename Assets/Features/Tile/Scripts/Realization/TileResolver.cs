using Assets.Features.GameLogic.Scripts.Interfaces;
using Assets.Features.Tile.Scripts.Interfaces;
using Assets.Features.TilesLayout.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Features.Tile.Scripts.Realization
{
    public class TileResolver : ITileResolver
    {
        private readonly int _scorePerResolve;

        private IScoreHandler _scoreHandler;
        private ITileLayout _tileLayout;
        private ITileStorage<TileModel> _tileStorage;
        private ITileSpawner<TileModel> _tileSpawner;

        public TileResolver(
            IScoreHandler scoreHandler,
            ITileLayout tileLayout,
            ITileStorage<TileModel> tileStorage,
            ITileSpawner<TileModel> tileSpawner,
            int scorePerResolve)
        {
            _scoreHandler = scoreHandler;
            _scorePerResolve = scorePerResolve;
            _tileLayout = tileLayout;
            _tileStorage = tileStorage;
            _tileSpawner = tileSpawner;
        }

        public void Dispose()
        {
            _scoreHandler = null;
            _tileLayout = null;
            _tileStorage = null;
            _tileSpawner = null;
        }

        public UniTask<IEnumerable<Vector2Int>> Resolve(Vector2Int coords)
        {
            _scoreHandler.Score += _scorePerResolve;
            if (_tileStorage.TryGetTile(_tileLayout.TilesLayout[coords.x][coords.y], out var model))
            {
                _tileSpawner.Despawn(model);
                _tileStorage.RemoveTile(_tileLayout.TilesLayout[coords.x][coords.y]);
            }
            return new UniTask<IEnumerable<Vector2Int>>(new[] { coords });
        }
    }
}
