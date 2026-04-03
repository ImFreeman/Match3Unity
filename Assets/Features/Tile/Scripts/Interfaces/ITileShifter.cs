using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Assets.Features.Tile.Scripts.Interfaces
{
    public interface ITileShifter : IDisposable
    {
        public UniTask Shift(int tileId, Vector2Int to);
        public UniTask Shift(Vector2Int from, Vector2Int to);
    }
}
