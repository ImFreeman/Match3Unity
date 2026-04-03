using Assets.Features.UI.GameWindow.Scripts;
using Assets.Features.UI.Scripts.Realization;
using System;
using TMPro;
using UnityEngine;

public class UIGameWindow : UIWindow
{
    public event EventHandler PauseButtonPressed;

    [SerializeField] private TilesField _tilesField;
    [SerializeField] private RectTransform _tilesContainer;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _movesText;
    [SerializeField] private UIButton _pauseButton;

    public TilesField TilesField => _tilesField;
    public RectTransform TilesContainer => _tilesContainer;
    public string Score
    {
        get { return _scoreText.text; }
        set { _scoreText.text = value; }
    }

    public string Moves
    {
        get { return _movesText.text; }
        set { _movesText.text = value; }
    }

    protected override void OnShow()
    {
        _pauseButton.ButtonClicked += OnButtonClicked;
    }    

    protected override void OnHide()
    {
        _pauseButton.ButtonClicked -= OnButtonClicked;
    }

    private void OnButtonClicked(object sender, EventArgs e)
    {
        PauseButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    
}
