using Assets.Features.GameLogic.Scripts.Interfaces;
using Assets.Features.Tile.Scripts.Interfaces;
using Assets.Features.TilesLayout.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Features.GameLogic.Scripts.Realization
{
    public class GameField : IGameField
    {
        public event EventHandler<IEnumerable<Vector2Int>> LayoutUpdated;

        private ITileLayout _tileLayout;
        private IMatchFinder _matchFinder;
        private ITileGenerator _tileGenerator;
        private ITileShifter _shifter;
        private IMatchResolver _matchResolver;

        public GameField(
            IMatchFinder matchFinder,
            ITileGenerator tileGenerator,
            ITileLayout tileLayout,
            ITileShifter shifter,
            IMatchResolver matchResolver)
        {
            _tileLayout = tileLayout;
            _matchFinder = matchFinder;
            _tileGenerator = tileGenerator;
            _shifter = shifter;
            _matchResolver = matchResolver;
        }

        public void Dispose()
        {
            _matchFinder = null;
            _tileGenerator = null;
        }

        public async UniTask<IEnumerable<Vector2Int>> ResolveTiles(IEnumerable<Vector2Int> checkedTiles)
        {
            return await _matchResolver.ResolveMatch(checkedTiles);
        }

        public async UniTask<IEnumerable<Vector2Int>> CheckTile(Vector2Int tile)
        {
            return await _matchFinder.FindMatch(tile, _tileLayout.TilesLayout);            
        }
        public async UniTask<IEnumerable<Vector2Int>> UpdateTiles(IEnumerable<Vector2Int> tilesToUpdate)
        {
            var changes = GetChanges(tilesToUpdate);
            var delta = changes.delta;
            var addTilesCount = changes.addTilesCount;

            HashSet<Vector2Int> tilesToCheck = new HashSet<Vector2Int>();
            var tasks = new List<UniTask>();

            for (int i = 0; i < delta.Length; i++)
            {
                for (int j = delta[i].Length - 1; j >= 0; j--)
                {
                    if (delta[i][j] > 0)
                    {
                        var from = new Vector2Int(i, j);
                        var to = new Vector2Int(i, j + delta[i][j]);
                        tasks.Add(_shifter.Shift(from, to));
                        _tileLayout.TilesLayout[i][j + delta[i][j]] = _tileLayout.TilesLayout[i][j];
                        tilesToCheck.Add(to);
                    }
                }

                for (int k = 0; k < addTilesCount[i]; k++)
                {
                    var newTileId = _tileGenerator.GenerateTile(null);
                    tasks.Add(_shifter.Shift(newTileId, new Vector2Int(i, k)));
                    _tileLayout.TilesLayout[i][k] = newTileId;
                    tilesToCheck.Add(new Vector2Int(i, k));
                }
            }

            await UniTask.WhenAll(tasks);

            LayoutUpdated?.Invoke(this, tilesToCheck);
            return tilesToCheck;
        }

        

        private (int[][] delta, int[] addTilesCount) GetChanges(IEnumerable<Vector2Int> tilesToUpdate)
        {
            int[][] delta = new int[_tileLayout.TilesLayout.Length][];
            for (int i = 0; i < delta.Length; i++)
            {
                delta[i] = new int[_tileLayout.TilesLayout[i].Length];
            }

            int[] addTilesCount = new int[_tileLayout.TilesLayout.Length];

            foreach (var tile in tilesToUpdate)
            {
                for (int i = 0; i < tile.y; i++)
                {
                    if (!tilesToUpdate.Contains(new Vector2Int(tile.x, i)))
                    {
                        delta[tile.x][i] += 1 + delta[tile.x][tile.y];
                    }
                }

                delta[tile.x][tile.y] = 0;
                addTilesCount[tile.x]++;
            }

            return (delta, addTilesCount);
        }

        
    }
}
