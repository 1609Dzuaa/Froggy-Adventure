using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public static class EventsManager
{
    private static Dictionary<EEvents, Action<object>> _dictEvents = new();

    public static void SubcribeToAnEvent(EEvents eventName, Action<object> callback)
    {
        if (!_dictEvents.ContainsKey(eventName))
        {
            _dictEvents.Add(eventName, callback);
            return;
            //Debug.Log("Added");
        }
        _dictEvents[eventName] += callback;
    }

    public static void UnsubscribeToAnEvent(EEvents eventName, Action<object> callback)
    {
        _dictEvents[eventName] -= callback;
    }

    public static void NotifyObservers(EEvents eventName, object eventArgsType = null)
    {
        _dictEvents[eventName]?.Invoke(eventArgsType);
    }
}
