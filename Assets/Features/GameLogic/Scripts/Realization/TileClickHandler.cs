using Assets.Features.GameLogic.Scripts.Interfaces;
using System;
using System.Collections.Generic;

namespace Assets.Features.GameLogic.Scripts.Realization
{
    public class TileClickHandler : IDisposable
    {
        private GameWindowPresentor _gameWindow;
        private IGameField _gameField;

        private bool _inputLocked;

        public TileClickHandler(
            IGameField gameField,
            GameWindowPresentor gameWindow)
        {
            _gameField = gameField;
            _gameWindow = gameWindow;

            _gameWindow.ClickOnTile += OnClickOnTile;
            _gameField.LayoutUpdated += OnLayoutUpdated;
        }

        public void Dispose()
        {
            _gameWindow.ClickOnTile -= OnClickOnTile;
            _gameField.LayoutUpdated -= OnLayoutUpdated;
        }

        private async void OnClickOnTile(object sender, UnityEngine.Vector2Int e)
        {
            if(_inputLocked)
            {
                return;
            }
            _inputLocked = true;
            var checkedTiles = await _gameField.CheckTile(e);
            var resolvedTiles = await _gameField.ResolveTiles(checkedTiles);
            await _gameField.UpdateTiles(resolvedTiles);
            _inputLocked = false;
        }

        private void OnLayoutUpdated(object sender, IEnumerable<UnityEngine.Vector2Int> e)
        {
            _gameWindow.UpdateView();
        }

        
    }
}
