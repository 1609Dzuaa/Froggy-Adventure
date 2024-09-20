using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public class ButtonSoundController : ButtonToggleController
{
    public override void OnClick(bool isOn)
    {
        base.OnClick(isOn);
        if (isOn)
            SoundsManager.Instance.MusicSource.Pause();
        else
            SoundsManager.Instance.MusicSource.UnPause();

        //SoundsManager.Instance.PlaySfx(SFXName, 1.0f);
    }
}
