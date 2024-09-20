using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public class ButtonToggleController : ButtonController
{
    [SerializeField] EToggleButton _btnName;

    public virtual void OnClick(bool isOn)
    {
        base.OnClick();
        UIManager.Instance.ToggleButtonOnClick(_btnName, isOn);
    }
}
