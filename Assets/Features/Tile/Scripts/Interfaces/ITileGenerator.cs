using System;

namespace Assets.Features.Tile.Scripts.Interfaces
{
    public interface ITileGenerator : IDisposable
    {
        public void GenerateTilesLayout(int columnsCount, int rowsCount, ITileResolver defaultTileResolver);
        public int GenerateTile(ITileResolver defaultTileResolver);
        public void ShuffleTiles();

    }
}
