using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Assets.Features.Tile.Scripts.Interfaces
{
    public interface ITileShifter : IDisposable
    {
        public UniTaskVoid Shift(int tileId, Vector2 to);
        public UniTaskVoid Shift(Vector2 from, Vector2 to);
    }
}
