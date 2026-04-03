using Assets.Features.GameLogic.Scripts.Interfaces;
using Assets.Features.Tile.Scripts.Interfaces;
using Assets.Features.Tile.Scripts.Realization;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Features.GameLogic.Scripts.Realization
{
    public class MatchFinder : IMatchFinder
    {
        private ITileStorage<TileModel> _tileStorage;
        private IEnumerable<Vector2Int> _searchMatrix;

        private bool[][] _deltaMatrix;

        public MatchFinder(ITileStorage<TileModel> tileStorage, IEnumerable<Vector2Int> searchMatrix)
        {
            _tileStorage = tileStorage;
            _searchMatrix = searchMatrix;
        }

        public void Dispose()
        {

        }

        public async UniTask<IEnumerable<Vector2Int>> FindMatch(Vector2Int origin, int[][] matrix)
        {
            PrepareDeltaMatrix(matrix);
            _deltaMatrix[origin.x][origin.y] = true;

            var resultList = new List<Vector2Int>
            {
                origin
            };
            HandlePoint(origin, _deltaMatrix, matrix, resultList);           

            return resultList;
        }

        private void PrepareDeltaMatrix(int[][] matrix)
        {
            if (_deltaMatrix == null)
            {
                _deltaMatrix = new bool[matrix.Length][];
                for (int i = 0; i < matrix.Length; i++)
                {
                    _deltaMatrix[i] = new bool[matrix[i].Length];
                }
            }
            else
            {
                for (int i = 0; i < _deltaMatrix.Length; i++)
                {
                    for (int j = 0; j < _deltaMatrix[i].Length; j++)
                    {
                        _deltaMatrix[i][j] = false;
                    }
                }
            }
        }

        private void HandlePoint(Vector2Int point, bool[][] deltaMatrix, int[][] matrix, IList<Vector2Int> resultList)
        {
            if (!_tileStorage.TryGetTile(matrix[point.x][point.y], out var originModel))
            {
                return;
            }            
            foreach(Vector2Int search in _searchMatrix)
            {
                var targetPoint = point + search;
                if(
                    targetPoint.x < 0 
                    || targetPoint.x >= deltaMatrix.Length 
                    || targetPoint.y < 0 
                    || targetPoint.y >= deltaMatrix[targetPoint.x].Length
                    || deltaMatrix[targetPoint.x][targetPoint.y])
                {
                    continue;
                }
                if (_tileStorage.TryGetTile(matrix[targetPoint.x][targetPoint.y], out var targetModel))
                {
                    if(CheckTileModels(originModel, targetModel))
                    {
                        resultList.Add(targetPoint);
                        deltaMatrix[targetPoint.x][targetPoint.y] = true;
                        HandlePoint(targetPoint, deltaMatrix, matrix, resultList);
                    }
                }
            }
        }

        private bool CheckTileModels(TileModel first, TileModel second)
        {
            return first.Type == second.Type;
        }
    }
}