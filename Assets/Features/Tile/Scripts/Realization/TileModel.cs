using Assets.Features.Tile.Scripts.Interfaces;

namespace Assets.Features.Tile.Scripts.Realization
{
    public class TileModel : ITileModel<TileType>
    {
        public TileType Type { get; set; }
        public ITileResolver TileResolver { get; set; }

        public void Dispose()
        {
            TileResolver = null;
        }
    }

    public enum TileType
    {
        Apple,
        Banana,
        Blueberry,
        Grape,
        Orange,
        Pear,
        Strawberry
    }
}
