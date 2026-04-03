using Assets.Features.Tile.Scripts.Interfaces;
using System;
using System.Collections.Generic;

namespace Assets.Features.Tile.Scripts.Realization
{
    public class TileStorage<TModel> : ITileStorage<TModel> where TModel : ITileModel
    {
        public event EventHandler<int> TileAdded;
        public event EventHandler<int> TileRemoved;

        private IDictionary<int, TModel> _models = new Dictionary<int, TModel>();

        private int _currentId;

        public void Dispose()
        {
            _models.Clear();
            _models = null;
        }

        public int AddTile(TModel model)
        {
            if (_models.TryAdd(_currentId + 1, model))
            {
                _currentId++;
                TileAdded?.Invoke(this, _currentId);
                return _currentId;
            }

            return -1;
        }

        public TModel? GetTile(int id)
        {
            if (_models.TryGetValue(id, out var model))
            {
                return model;
            }

            return default;
        }

        public bool TryGetTile(int id, out TModel model)
        {
            return _models.TryGetValue(id, out model);
        }

        public void RemoveTile(int id)
        {
            if (_models.ContainsKey(id))
            {
                TileRemoved?.Invoke(this, id);
                _models.Remove(id);
            }
        }
    }
}
