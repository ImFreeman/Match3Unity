using System;
using UnityEngine;

public class UIButton : MonoBehaviour
{
    public event EventHandler ButtonClicked;

    protected void ProccedClick()
    {
        ButtonClicked?.Invoke(this, EventArgs.Empty);
    }
}
