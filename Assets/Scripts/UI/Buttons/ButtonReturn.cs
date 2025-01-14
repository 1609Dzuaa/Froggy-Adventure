using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;
using static GameConstants;

public class ButtonReturn : ButtonController
{
    public override void OnClick()
    {
        base.OnClick();
        string content = "Are You Sure Want To Back To Main Menu ?";
        NotificationParam param = new(content, false, null, () =>
        {
            UIManager.Instance.AnimateAndTransitionScene(GAME_MENU, true, false, false, true);
            EventsManager.NotifyObservers(EEvents.OnReturnMainMenu);
            GameManager.Instance.ListPrefsInconsistentKeys.Clear();
        });
        ShowNotificationHelper.ShowNotification(param);
    }
}
