using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public static class ShowNotificationHelper
{
    public static void ShowNotification(NotificationParam param)
    {
        UIManager.Instance.TogglePopup(EPopup.Notification, true);
        EventsManager.NotifyObservers(EEvents.NotificationOnPopup, param);
    }
}
