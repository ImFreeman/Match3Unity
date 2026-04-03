using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Features.TilesLayout.Scripts.Interfaces
{
    public interface ITileLayout : IDisposable
    {
        public int[][] TilesLayout { get; set; }
        public IEnumerable<Vector2Int> GetIndexes();
    }
}
