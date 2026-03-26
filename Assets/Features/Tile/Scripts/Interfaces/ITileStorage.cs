using System;

namespace Assets.Features.Tile.Scripts.Interfaces
{
    public interface ITileStorage<TModel> : IDisposable where TModel : ITileModel
    {
        public event EventHandler<int> TileAdded;
        public event EventHandler<int> TileRemoved;
        public bool TryGetTile(int id, out TModel model);
        public TModel GetTile(int id);
        public int AddTile(TModel model);
        public void RemoveTile(int id);
    }
}
