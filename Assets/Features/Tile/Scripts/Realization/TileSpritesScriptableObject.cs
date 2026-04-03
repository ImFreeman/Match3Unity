using Assets.Features.Tile.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Features.Tile.Scripts.Realization
{
    [CreateAssetMenu(fileName = "TileSprites", menuName = "ScriptableObjects/TileSprites", order = 1)]
    public class TileSpritesScriptableObject : ScriptableObject
    {
        public IReadOnlyDictionary<TileType, Sprite> TileTypeToSpriteData
        {
            get
            {
                if(!_inited)
                {
                    Init();
                }

                return _tileTypeToSpriteData;
            }
        }

        [SerializeField] private TileTypeSpriteData[] _data;

        [NonSerialized] private bool _inited;

        private Dictionary<TileType, Sprite> _tileTypeToSpriteData = new Dictionary<TileType, Sprite>();

        private void Init()
        {
            if(_inited) return;

            foreach (var item in _data)
            {
                _tileTypeToSpriteData.Add(item.Type, item.Sprite);
            }

            _inited = true;
        }

        [Serializable]
        public struct TileTypeSpriteData
        {
            public TileType Type;
            public Sprite Sprite;
        }
    }    
}
