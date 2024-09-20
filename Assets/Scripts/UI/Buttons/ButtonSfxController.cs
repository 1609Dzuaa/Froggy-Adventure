using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSfxController : ButtonToggleController
{
    public override void OnClick(bool isOn)
    {
        base.OnClick(isOn);
        if (isOn)
            SoundsManager.Instance.SFXSource.mute = true;
        else
            SoundsManager.Instance.SFXSource.mute = false;
    }
}

