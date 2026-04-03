using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Features.Tile.Scripts.Interfaces
{
    public interface ITileResolver : IDisposable
    {
        public UniTask<IEnumerable<Vector2Int>> Resolve(Vector2Int coords);
    }
}
