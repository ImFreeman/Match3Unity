using Assets.Features.Tile.Scripts.Interfaces;
using System;

namespace Assets.Features.Tile.Scripts
{
    public interface ITileModel : IDisposable
    {
        public ITileResolver TileResolver { get; set; }
    }

    public interface ITileModel<T> : ITileModel where T : Enum
    {
        public T Type { get; set; }
    }
}
