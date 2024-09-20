using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

//Phục vụ cho 1 số button có toggle UI
public class ButtonPopupController : ButtonController
{
    [SerializeField] EPopup _popupController;

    public void OnClick(bool isOn)
    {
        base.OnClick();
        UIManager.Instance.TogglePopup(_popupController, isOn);
    }
}
