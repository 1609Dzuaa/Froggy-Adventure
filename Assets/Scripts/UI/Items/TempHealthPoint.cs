using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameConstants;

public class TempHealthPoint : ItemShop
{
    public override bool HandleBuyItem()
    {
        if (PlayerHealthManager.Instance.CurrentHP + PlayerHealthManager.Instance.TempHP < PlayerHealthManager.Instance.MaxHP)
        {
            PlayerHealthManager.Instance.ChangeHPState(HP_STATE_TEMP);
            return true;
        }

        string content = "Purchase Fail,\nMaximum Temp Health Point Reached!";
        NotificationParam param = new(content, true);
        ShowNotificationHelper.ShowNotification(param);

        return false;
    }
}
