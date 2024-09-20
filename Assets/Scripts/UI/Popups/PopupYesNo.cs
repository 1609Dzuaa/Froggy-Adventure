using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameConstants;

public class PopupYesNo : PopupController
{
    protected override void OnEnable()
    {
        base.OnEnable();
        //if(!_isFirstOnEnable) Time.timeScale = 0f;
    }

    public void ButtonYesOnClick()
    {
        base.OnClose();
        Time.timeScale = 1f;
        UIManager.Instance.AnimateAndTransitionScene(GAME_MENU);
    }

    public override void OnClose()
    {
        base.OnClose();
        Time.timeScale = 1f;
    }
}
