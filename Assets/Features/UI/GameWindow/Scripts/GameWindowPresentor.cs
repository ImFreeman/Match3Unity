using Assets.Features.GameLogic.Scripts.Interfaces;
using Assets.Features.Tile.Scripts.Interfaces;
using Assets.Features.Tile.Scripts.Realization;
using Assets.Features.TilesLayout.Scripts.Interfaces;
using Assets.Features.UI.PauseWindow.Scripts;
using Assets.Features.UI.UIGraphicElement.Scripts.Interfaces;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameWindowPresentor : IDisposable
{
    public event EventHandler<Vector2Int> ClickOnTile;

    private IScoreHandler _scoreHandler;
    private IMovesHandler _movesHandler;
    private ITileLayout _tileLayout;
    private ITileStorage<TileModel> _tileStorage;
    private IUIGraphicElementsSpawner<UIGraphicElementImage, UIGraphicElementProtocol> _tileViewSpawner;
    private IReadOnlyDictionary<TileType, Sprite> _tileSprites;
    private IUIGraphicElementStorage<UIGraphicElementImage> _viewStorage;
    private UIGameWindow _gameWindow;
    private IUIService _uiService;

    private IList<Tween> _tweens;
    private bool _isInputEnabled = true;
    public GameWindowPresentor(
        ITileStorage<TileModel> tileStorage,
        IUIGraphicElementsSpawner<UIGraphicElementImage, UIGraphicElementProtocol> tileViewSpawner,
        ITileLayout tileLayout,
        ITileSprites tileSprites,
        IUIService service,
        IScoreHandler scoreHandler,
        IMovesHandler movesHandler,
        IUIGraphicElementStorage<UIGraphicElementImage> viewStorage)
    {
        _uiService = service;
        _tileStorage = tileStorage;
        _tileViewSpawner = tileViewSpawner;
        _tileLayout = tileLayout;
        _tileSprites = tileSprites.TileTypeToSpriteData;
        _scoreHandler = scoreHandler;
        _movesHandler = movesHandler;
        _viewStorage = viewStorage;
        _gameWindow = service.Get<UIGameWindow>();

        _tweens = new List<Tween>();
        
        _gameWindow.TilesField.PointerDown += OnPointerDown;
        _gameWindow.PauseButtonPressed += OnPauseButtonPressed;

        _tileStorage.TileAdded += OnTileAdded;
        _tileStorage.TileRemoved += OnTileRemoved;

        _scoreHandler.ScoreUpdated += OnScoreUpdated;
        _movesHandler.MovesCountUpdated += OnMovesCountUpdated;               
    }    

    public void Dispose()
    {
        _tileStorage.TileAdded -= OnTileAdded;
        _tileStorage.TileRemoved -= OnTileRemoved;

        _scoreHandler.ScoreUpdated -= OnScoreUpdated;
        _movesHandler.MovesCountUpdated -= OnMovesCountUpdated;

        _gameWindow.TilesField.PointerDown -= OnPointerDown;
        _gameWindow.PauseButtonPressed -= OnPauseButtonPressed;

        _tileStorage = null;        
        _tileLayout = null;
        _tileSprites = null;
        _scoreHandler = null;
        _movesHandler = null;
        _gameWindow = null;
        _viewStorage = null;
        _tileViewSpawner = null;

        foreach (var tween in _tweens)
        {
            tween.Kill();            
        }
        _tweens.Clear();
        _tweens = null;             
    }

    public void SetInputEnabled(bool value)
    {
        _isInputEnabled = value;
    }

    public void UpdateView()
    {
        for (int i = 0; i < _tileLayout.TilesLayout.Length; i++)
        {
            for (int j = 0; j < _tileLayout.TilesLayout[i].Length; j++)
            {
                if (_viewStorage.Items.TryGetValue(_tileLayout.TilesLayout[i][j], out var tileView))
                {                    
                    tileView.RectTransform.SetParent(_gameWindow.TilesContainer);
                    var newX = tileView.RectTransform.rect.width * i;
                    var newY = tileView.RectTransform.rect.height * j * -1;
                    tileView.RectTransform.anchoredPosition = new Vector2(newX, newY);
                }
            }
        }
    }    

    public Vector2Int GetLayoutPositionFromPosition(Vector2 position)
    {
        var tileSize = GetTileSize();
        var row = (int)(position.y / tileSize.y);
        var column = (int)(position.x / tileSize.x);
        
        if (column >= 0 && column < _tileLayout.TilesLayout.Length && row >= 0 && row < _tileLayout.TilesLayout[column].Length)
        {
            return new Vector2Int(column, row);
        }
        return new Vector2Int(-1, -1);
    }

    public Vector2 GetPositionFromLayoutPosition(Vector2Int layoutPosition)
    {
        var tileSize = GetTileSize();
        var posX = layoutPosition.x * tileSize.x;
        var posY = layoutPosition.y * tileSize.y * -1;

        return new Vector2(posX, posY);
    }

    private void OnPauseButtonPressed(object sender, EventArgs e)
    {
        _uiService.Show<UIPauseWindow>();
    }
    private void OnPointerDown(object sender, Vector2 e)
    {
        if(!_isInputEnabled)
        {
            return;
        }
        var tilePlace = GetLayoutPositionFromPosition(e);
        if (tilePlace.x == -1 || tilePlace.y == -1)
        {
            return;
        }
        ClickOnTile?.Invoke(this, tilePlace);
    }

    private void OnMovesCountUpdated(object sender, int e)
    {
        _gameWindow.Moves = e.ToString();
    }

    private void OnScoreUpdated(object sender, int e)
    {
        _gameWindow.Score = e.ToString();
    }

    private Vector2 GetTileSize()
    {
        var fieldWidth = _gameWindow.TilesContainer.rect.width;
        var columnsCount = _tileLayout.TilesLayout.Length;
        var tileWidth = fieldWidth / columnsCount;
        return new Vector2(tileWidth, tileWidth);
    }

    private void OnTileAdded(object sender, int e)
    {
        if(_tileStorage.TryGetTile(e, out TileModel tile))
        {
            var sprite = GetTileSprite(tile.Type);
            var instance = _tileViewSpawner.Spawn(new UIGraphicElementProtocol(sprite, GetTileSize()));
            instance.RectTransform.SetParent(_gameWindow.TilesContainer);
            if (!_viewStorage.TryAdd(e,instance))
            {
                throw new Exception("Tile view with this id already exsist");
            }
        }
    }

    private void OnTileRemoved(object sender, int e)
    {
        if(_viewStorage.Items.TryGetValue(e, out var view))
        {
            var tween = view.RectTransform.DOScale(0.0f, 0.5f).SetEase(Ease.Flash).OnComplete(() => 
            {
                _tileViewSpawner.Despawn(view);
                _viewStorage.Remove(e);
            });        
            _tweens.Add(tween);
        }
    }

    private Sprite GetTileSprite(TileType type)
    {
        if(_tileSprites.TryGetValue(type, out var sprite))
        {
            return sprite;
        }
        return null;
    }    
}
