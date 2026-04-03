using Assets.Features.Core.Command.Realization;
using Assets.Features.Core.ServiceLocatorScript;
using Assets.Features.GameLogic.Scripts.Interfaces;
using Assets.Features.SaveSystem.Scripts.Interfaces;
using Assets.Features.TilesLayout.Scripts.Interfaces;
using Assets.Features.UI.RecordsWindow.Scripts.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Features.GameLogic.Scripts.Realization
{
    public class GameFlow : IDisposable
    {
        private IMovesHandler _movesHandler;
        private IScoreHandler _scoreHandler;
        private IGameField _gameField;
        private ITileLayout _tileLayout;
        private IRecordsTrack _recordsTrack;
        private IUIService _uiService;

        private ICommand _leaveCommand;

        private readonly Color _recordHighlightColor;
        private readonly int _recordsSceneId;
        private readonly int _minMatchSize;
        public GameFlow(
            IMovesHandler movesHandler,
            IGameField gameField,
            ITileLayout tileLayout,
            IRecordsTrack recordsTrack,
            IScoreHandler scoreHandler,
            IUIService service,
            int minMatchSize,
            int recordsSceneId,
            Color recordHighlightColor)
        {
            _uiService = service;
            _movesHandler = movesHandler;
            _gameField = gameField;
            _tileLayout = tileLayout;
            _minMatchSize = minMatchSize;
            _recordsTrack = recordsTrack;
            _scoreHandler = scoreHandler;
            _recordsSceneId = recordsSceneId;
            _recordHighlightColor = recordHighlightColor;

            _leaveCommand = new LeaveGameSceneCommand(_recordsSceneId);
            
            _gameField.LayoutUpdated += OnLayoutUpdated;
            
        }

        public void Dispose()
        {
            _leaveCommand.Dispose();
            _leaveCommand = null;

            _gameField.LayoutUpdated -= OnLayoutUpdated;

            _movesHandler = null;
            _gameField = null;
            _tileLayout = null;
            _scoreHandler = null;
            _uiService = null;
            _recordsTrack = null;
        }

        private async void OnLayoutUpdated(object sender, IEnumerable<Vector2Int> e)
        {
            if(_movesHandler.MovesCount <= 0)
            {
                EndGame();
                return;
            }
            var tilesIndexes = _tileLayout.GetIndexes();
            foreach (var tileIndex in tilesIndexes)
            {
                var match = await _gameField.CheckTile(tileIndex);
                if(match.Count() >= _minMatchSize)
                {                    
                    return;
                }
            }
            EndGame();
        }                

        private void EndGame()
        {
            if(!_recordsTrack.CheckScore(_scoreHandler.Score))
            {
                _uiService.Show<UIGameOverPopUp>();
                return;
            }
            var data = new SaveSystem.Scripts.RecordData() { Date = DateTime.Now, Value = _scoreHandler.Score };
            _recordsTrack.AddRecord(data);
            ServiceLocator.Register(new SetRecordHighlightedCommand(data, _recordHighlightColor));
            _leaveCommand.Do();
        }        
    }
}
