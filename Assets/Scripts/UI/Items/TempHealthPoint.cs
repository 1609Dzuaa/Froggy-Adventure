using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public class TempHealthPoint : ItemShop
{
    public override bool HandleBuyItem()
    {
        //you can only buy temp hp only if you have minimum 1 hp
        string content;
        NotificationParam param;

        if (PlayerHealthManager.Instance.CurrentHP == 0) 
        {
            content = "Purchase Fail,\nYou must have at least 1 HP to buy Temp Health Point!";
            param = new(content, true);
            ShowNotificationHelper.ShowNotification(param);
            return false;
        }

        if (PlayerHealthManager.Instance.CurrentHP + PlayerHealthManager.Instance.TempHP < PlayerHealthManager.Instance.MaxHP)
        {
            EventsManager.NotifyObservers(EEvents.OnChangeHP, EHPStatus.AddOneTempHP);
            return true;
        }

        content = "Purchase Fail,\nMaximum Temp Health Point Reached!";
        param = new(content, true);
        ShowNotificationHelper.ShowNotification(param);

        return false;
    }
}
