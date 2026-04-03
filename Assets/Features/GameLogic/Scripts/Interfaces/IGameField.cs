using Assets.Features.Tile.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Features.GameLogic.Scripts.Interfaces
{
    public interface IGameField : IDisposable
    {
        public event EventHandler<IEnumerable<Vector2Int>> LayoutUpdated;
        public UniTask<IEnumerable<Vector2Int>> CheckTile(Vector2Int tilesCoord);
        public UniTask<IEnumerable<Vector2Int>> ResolveTiles(IEnumerable<Vector2Int> checkedTiles);
        public UniTask<IEnumerable<Vector2Int>> UpdateTiles(IEnumerable<Vector2Int> tilesToUpdate);
    }
}
