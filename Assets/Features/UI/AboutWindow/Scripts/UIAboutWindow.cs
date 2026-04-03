using Assets.Features.UI.Scripts.Realization;
using System;
using UnityEngine;

public class UIAboutWindow : UIWindow
{
    public event EventHandler LinkButtonPressed;
    public event EventHandler BackButtonPressed;

    [SerializeField] private UIButton _linkButton;
    [SerializeField] private UIButton _backButton;
    protected override void OnShow()
    {
        _linkButton.ButtonClicked += OnLinkButtonClicked;
        _backButton.ButtonClicked += OnBackButtonClicked;
    }

    protected override void OnHide()
    {
        _linkButton.ButtonClicked -= OnLinkButtonClicked;
        _backButton.ButtonClicked -= OnBackButtonClicked;
    }

    private void OnBackButtonClicked(object sender, EventArgs e)
    {
        BackButtonPressed?.Invoke(this, e);
    }

    private void OnLinkButtonClicked(object sender, EventArgs e)
    {
        LinkButtonPressed?.Invoke(this, e);
    }
}
