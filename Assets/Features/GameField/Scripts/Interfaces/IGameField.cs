using Assets.Features.Tile.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Features.GameField.Scripts.Interfaces
{
    public interface IGameField : IDisposable
    {
        public event EventHandler<IEnumerable<Vector2>> LayoutUpdated;
        public void SetMatchResolvers(IReadOnlyDictionary<int, IMatchResolver> matchResolvers);
        public void SetDefaultTileResolver(ITileResolver resolver);
        public UniTask<IEnumerable<Vector2>> CheckTiles(IEnumerable<Vector2> tilesCoord);
        public UniTask<IEnumerable<Vector2>> UpdateTiles(IEnumerable<Vector2> tilesToUpdate);
    }
}
