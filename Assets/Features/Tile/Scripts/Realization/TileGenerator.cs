using Assets.Features.Tile.Scripts.Interfaces;
using Assets.Features.TilesLayout.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Features.Tile.Scripts.Realization
{
    public class TileGenerator<TModel, TEnum> : ITileGenerator
        where TEnum : Enum
        where TModel : ITileModel<TEnum>
    {
        private ITileSpawner<TModel, TEnum> _spawner;
        private ITileStorage<TModel> _storage;
        private ITileResolver _defaultTileResolver;
        private ITileLayout _tileLayout;
        private IEnumerable<(Vector2Int, TEnum)> _staticTiles;

        private System.Random _random = new System.Random();
        private Array _enums = Enum.GetValues(typeof(TEnum));        

        public TileGenerator(
            ITileStorage<TModel> storage,
            ITileSpawner<TModel, TEnum> spawner,
            ITileLayout tileLayout,
            ITileResolver defaultTileResolver,
            IEnumerable<(Vector2Int, TEnum)> staticTiles)
        {
            _storage = storage;
            _spawner = spawner;
            _defaultTileResolver = defaultTileResolver;
            _tileLayout = tileLayout;
            _staticTiles = staticTiles;
        }

        public void Dispose()
        {
            _random = null;
            _enums = null;

            _spawner = null;
            _storage = null;
            _defaultTileResolver = null;
            _tileLayout = null;
            _staticTiles = null;
        }
        public void ShuffleTiles()
        {
            for (int i = 0; i < _tileLayout.TilesLayout.Length; i++)
            {
                for (int j = 0; j < _tileLayout.TilesLayout[i].Length; j++)
                {
                    if (_storage.TryGetTile(_tileLayout.TilesLayout[i][j], out var model))
                    {
                        model.Type = (TEnum)_enums.GetValue(_random.Next(_enums.Length));
                        model.TileResolver = _defaultTileResolver;
                    }
                }
            }            
        }

        public void GenerateTilesLayout(int columnsCount, int rowsCount, ITileResolver defaultTileResolver)
        {
            ITileResolver tileResolver = defaultTileResolver == null ? _defaultTileResolver : defaultTileResolver;
            _tileLayout.TilesLayout = new int[columnsCount][];
            for (int i = 0; i < _tileLayout.TilesLayout.Length; i++)
            {
                _tileLayout.TilesLayout[i] = new int[rowsCount];
                for (int j = 0; j < rowsCount; j++)
                {
                    bool isStatic = false;
                    foreach (var item in _staticTiles)
                    {
                        if(item.Item1.x == i && item.Item1.y == j)
                        {
                            _tileLayout.TilesLayout[i][j] = SpawnTile(item.Item2, tileResolver);
                            isStatic = true;
                            break;
                        }
                    }
                    if(!isStatic)
                    {
                        _tileLayout.TilesLayout[i][j] = GenerateTile(tileResolver);
                    }                    
                }
            }
        }

        public int GenerateTile(ITileResolver defaultTileResolver = null)
        {
            TEnum type = (TEnum)_enums.GetValue(_random.Next(_enums.Length));
            return SpawnTile(type, defaultTileResolver);            
        }

        private int SpawnTile(TEnum type, ITileResolver defaultTileResolver = null)
        {
            return _storage.AddTile(_spawner.Spawn(type, defaultTileResolver != null ? defaultTileResolver : _defaultTileResolver));
        }
    }
}
