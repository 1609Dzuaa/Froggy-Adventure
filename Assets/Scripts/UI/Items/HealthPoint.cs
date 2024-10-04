using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPoint : ItemShop
{
    public override bool HandleBuyItem()
    {
        if (PlayerHealthManager.Instance.CurrentHP == 3)
        {
            string content = "Purchase Fail,\nMaximum Health Point Reached!";
            NotificationParam param = new(content, true);
            ShowNotificationHelper.ShowNotification(param);
            _isPurchaseSuccess = false;
            return _isPurchaseSuccess;
        }

        PlayerHealthManager.Instance.CurrentHP = Mathf.Clamp(++PlayerHealthManager.Instance.CurrentHP, 0, 3);
        return true;
    }
}
