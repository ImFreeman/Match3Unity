using Assets.Features.Core.Command.Realization;
using Assets.Features.Core.ServiceLocatorScript;
using Assets.Features.SaveSystem.Scripts.Commands;
using Assets.Features.Tile.Scripts.Commands;
using Assets.Features.Tile.Scripts.Realization;
using Assets.Features.UI.PauseWindow.Scripts.Commands;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Features.Core.ApplicationLauncher
{
    public class GameSceneLauncher : CompositeApplicationLauncher
    {
        [SerializeField] private string _prefsKey;
        [SerializeField] private int _scorePerTile;
        [SerializeField] private int _numberOfColumns;
        [SerializeField] private int _numberOfRows;
        [SerializeField] private int _initPoolCapacity;
        [SerializeField] private UIGraphicElementImage _tilePrefab;
        [SerializeField] private TileSpritesScriptableObject _tileSprites;
        [SerializeField] private Vector2Int[] _searchMatrix;
        [SerializeField] private TileData[] _staticTiles;
        [SerializeField] private Color _flickeringColor;
        [SerializeField] private float _flickeringDuration;
        [SerializeField] private MoveAdditionData[] _moveAdditionData;
        [SerializeField] private int _mainMenuSceneId;
        [SerializeField] private int _recordSceneId;
        [SerializeField] private int _defaultMovesCount;
        [SerializeField] private int _minMatchSize;
        [SerializeField] private Color _recordHighlight;
        

        protected override IEnumerable<ICommand> GetCommands()
        {
            Debug.Log($"game get commands {GetInstanceID()}");
            var list = new List<(Vector2Int, TileType)>() { Capacity = _staticTiles.Length };
            foreach (var staticTile in _staticTiles)
            {
                list.Add((staticTile.Coords, staticTile.TileType));
            }

            var moveAdditionDict = new Dictionary<int, int>();
            foreach (var movesData in _moveAdditionData)
            {
                moveAdditionDict.Add(movesData.MatchLength, movesData.MovesToAdd);
            }

            var commands = new List<ICommand>(base.GetCommands())
            {
                new InitTileSystemsCommand(_initPoolCapacity, _tilePrefab, _tileSprites),
                new InitGameWindowCommand(),
                new InitGameLogicCommand(
                    _scorePerTile,
                    _numberOfColumns,
                    _numberOfRows,
                    _searchMatrix,
                    list,
                    moveAdditionDict,
                    _defaultMovesCount,
                    _minMatchSize,
                    _recordSceneId,
                    _recordHighlight,
                    _mainMenuSceneId
                    ),
                new InitPauseWindowCommand(_mainMenuSceneId),
                new StartGameSceneCommand(),
                new TilesFlickeringCommand(list, _flickeringColor, _flickeringDuration),
            };

            return commands;
        }

        private void OnApplicationQuit()
        {
            (new SaveRecordsCommand(_prefsKey)).Do();
            (new ClearGameSceneCommand()).Do();
            ServiceLocator.Clear();
        }

        [Serializable]
        private struct TileData
        {
            public Vector2Int Coords;
            public TileType TileType;
        }
        [Serializable]
        private struct MoveAdditionData
        {
            public int MatchLength;
            public int MovesToAdd;
        }
    }
}
