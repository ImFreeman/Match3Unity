using Assets.Features.GameLogic.Scripts.Interfaces;
using Assets.Features.Tile.Scripts.Interfaces;
using Assets.Features.Tile.Scripts.Realization;
using Assets.Features.TilesLayout.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Features.GameLogic.Scripts.Realization
{
    public class MatchResolver : IMatchResolver
    {
        private ITileLayout _tileLayout;
        private ITileStorage<TileModel> _tileStorage;
        private IMovesHandler _movesHandler;
        private IReadOnlyDictionary<int, int> _movesAdditionData;


        public MatchResolver(
            ITileStorage<TileModel> tileStorage,
            ITileLayout tileLayout,
            IMovesHandler movesHandler,
            IReadOnlyDictionary<int, int> movesAdditionData)
        {
            _tileStorage = tileStorage;
            _tileLayout = tileLayout;
            _movesHandler = movesHandler;
            _movesAdditionData = movesAdditionData;
        }

        public void Dispose()
        {
            
        }

        public async UniTask<IEnumerable<Vector2Int>> ResolveMatch(IEnumerable<Vector2Int> match)
        {            
            foreach (var item in match)
            {
                if (_tileStorage.TryGetTile(_tileLayout.TilesLayout[item.x][item.y], out var matchTile))
                {
                    await matchTile.TileResolver.Resolve(item);
                }
            }
            if(_movesAdditionData.TryGetValue(match.Count(), out var movesToAdd))
            {
                _movesHandler.MovesCount += movesToAdd;
            }
            else
            {
                _movesHandler.MovesCount--;
            }

            var list = new List<Vector2Int>(match);

            return list;
        }
    }
}
