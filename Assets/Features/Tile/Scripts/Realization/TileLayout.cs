using Assets.Features.TilesLayout.Scripts.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Features.TilesLayout.Scripts.Realization
{
    public class TileLayout : ITileLayout
    {
        public int[][] TilesLayout { get; set; }

        public void Dispose()
        {
            for (int i = 0; i < TilesLayout.Length; i++)
            {
                TilesLayout[i] = null;
            }
            TilesLayout = null;
        }

        public IEnumerable<Vector2Int> GetIndexes()
        {
            var indexes = new List<Vector2Int>();
            for (int i = 0; i < TilesLayout.Length; i++)
            {
                for (int j = 0; j < TilesLayout[i].Length; j++)
                {
                    indexes.Add(new Vector2Int(i, j));
                }
            }

            return indexes;
        }
    }
}
