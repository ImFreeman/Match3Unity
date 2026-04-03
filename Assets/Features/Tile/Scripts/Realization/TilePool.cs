using Assets.Features.Tile.Scripts.Interfaces;
using System;
using System.Collections.Generic;

namespace Assets.Features.Tile.Scripts.Realization
{
    public class TilePool : ITileSpawner<TileModel, TileType>
    {
        private readonly Stack<TileModel> _pool = new Stack<TileModel>();
        private readonly object _lock = new object();

        public TilePool(int initCapacity)
        {
            for (int i = 0; i < initCapacity; i++)
            {
                _pool.Push(CreateNew());
            }
        }

        public TileModel Spawn(TileType type, ITileResolver resolver)
        {
            TileModel model;
            lock (_lock)
            {
                if (_pool.Count > 0)
                {
                    model = _pool.Pop();
                }
                else
                {
                    model = CreateNew();
                }

                model.Type = type;
                model.TileResolver = resolver;
            }

            return model;
        }

        public void Despawn(TileModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            lock (_lock)
            {
                _pool.Push(model);
            }
        }

        public void Dispose()
        {
            lock (_lock)
            {
                foreach (var model in _pool)
                {
                    if (model is IDisposable disposable)
                        disposable.Dispose();
                }
                
                _pool.Clear();                
            }
        }

        private TileModel CreateNew()
        {
            return new TileModel();
        }
    }
}
