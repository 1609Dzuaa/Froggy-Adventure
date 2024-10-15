using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class TimeDisplayHelper
{
    public static TextMeshProUGUI DisplayTime(ref TextMeshProUGUI result, int TimeCompleted, int TimeAllow)
    {
        int minuteCompleted = TimeCompleted / 60;
        int secondCompleted = TimeCompleted % 60;
        int minuteAllow = TimeAllow / 60;
        int secondAllow = TimeAllow % 60;

        string formattedTimeCompleted = $"{minuteCompleted}:{secondCompleted:D2}";
        string formattedTimeAllow = $"{minuteAllow}:{secondAllow:D2}";

        result.text = formattedTimeCompleted + " / " + formattedTimeAllow;

        return result;
    }

    public static TextMeshProUGUI DisplayCooldownTime(ref TextMeshProUGUI result, float timeLeft, string decimalDisplayType)
    {
        result.text = timeLeft.ToString(decimalDisplayType);
        return result;
    }
}
