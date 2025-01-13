using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameConstants;

public class RareDiamond : ItemShop
{
    public override bool HandleBuyItem()
    {
        if (PlayerHealthManager.Instance.MaxHP == PLAYER_MAX_HP)
        {
            string content = "Purchase Fail,\nMaximum Health Point Reached!";
            NotificationParam param = new(content, true);
            ShowNotificationHelper.ShowNotification(param);
            _isPurchaseSuccess = false;
            return _isPurchaseSuccess;
        }

        PlayerHealthManager.Instance.IncreaseMaxHP();
        return true;
    }
}
