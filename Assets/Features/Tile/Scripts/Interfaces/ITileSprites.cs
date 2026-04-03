using Assets.Features.Tile.Scripts.Realization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Features.Tile.Scripts.Interfaces
{
    public interface ITileSprites : IDisposable
    {
        public IReadOnlyDictionary<TileType, Sprite> TileTypeToSpriteData { get; }
    }
}
