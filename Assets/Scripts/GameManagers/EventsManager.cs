using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public class EventsManager : BaseSingleton<EventsManager>
{
    private Dictionary<EEvents, Action<object>> _dictEvents = new();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void SubcribeToAnEvent(EEvents eventName, Action<object> callback)
    {
        if (!_dictEvents.ContainsKey(eventName))
        {
            _dictEvents.Add(eventName, callback);
            return;
            //Debug.Log("Added");
        }
        _dictEvents[eventName] += callback;
    }

    public void UnsubscribeToAnEvent(EEvents eventName, Action<object> callback)
    {
        _dictEvents[eventName] -= callback;
    }

    public void NotifyObservers(EEvents eventName, object eventArgsType = null)
    {
        _dictEvents[eventName]?.Invoke(eventArgsType);
    }
}
