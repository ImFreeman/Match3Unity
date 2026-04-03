using Assets.Features.Tile.Scripts.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Features.Tile.Scripts.Realization
{
    public class TileSpritesSOWrapper : ITileSprites
    {
        public IReadOnlyDictionary<TileType, Sprite> TileTypeToSpriteData => _so.TileTypeToSpriteData;

        private TileSpritesScriptableObject _so;

        public TileSpritesSOWrapper(TileSpritesScriptableObject so)
        {
            _so = so;
        }

        public void Dispose()
        {
            Resources.UnloadAsset(_so);
            _so = null;
        }
    }
}
