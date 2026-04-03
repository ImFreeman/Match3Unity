using Assets.Features.UI.Scripts.Realization;
using System;
using UnityEngine;

public class UIGameOverPopUp : UIWindow
{
    public event EventHandler OkButtonPressed;
    [SerializeField] private UIButton _okButton;

    protected override void OnShow()
    {
        _okButton.ButtonClicked += OnButtonClicked;
    }    

    protected override void OnHide()
    {
        _okButton.ButtonClicked -= OnButtonClicked;
    }

    private void OnButtonClicked(object sender, EventArgs e)
    {
        OkButtonPressed?.Invoke(this, EventArgs.Empty);
    }
}
