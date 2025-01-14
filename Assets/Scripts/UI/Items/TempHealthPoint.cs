using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public class TempHealthPoint : ItemShop
{
    public override bool HandleBuyItem()
    {
        if (PlayerHealthManager.Instance.CurrentHP + PlayerHealthManager.Instance.TempHP < PlayerHealthManager.Instance.MaxHP)
        {
            EventsManager.NotifyObservers(EEvents.OnChangeHP, EHPStatus.AddOneTempHP);
            return true;
        }

        string content = "Purchase Fail,\nMaximum Temp Health Point Reached!";
        NotificationParam param = new(content, true);
        ShowNotificationHelper.ShowNotification(param);

        return false;
    }
}
