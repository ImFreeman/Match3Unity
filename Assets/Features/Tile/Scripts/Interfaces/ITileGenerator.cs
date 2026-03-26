namespace Assets.Features.Tile.Scripts.Interfaces
{
    public interface ITileGenerator
    {
        public int[][] GenerateTilesLayout(int columnsCount, int rowsCount, ITileResolver defaultTileResolver);
        public int GenerateTile(ITileResolver defaultTileResolver);
        public void ShuffleTiles(int[][] tiles);

    }
}
