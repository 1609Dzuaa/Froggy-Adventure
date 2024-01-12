using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : MonoBehaviour 
{
    private static EventsManager _instance;
    //You can assign values of any type to variables of type object
    //https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/reference-types
    private Dictionary<GameEnums.EEvents, Action<object>> _dictEvents = new();

    public static EventsManager Instance
    {
        get 
        {
            if (!_instance)
                _instance = FindObjectOfType<EventsManager>();

            if (!_instance)
                Debug.Log("0 co EventsManager trong Scene");

            return _instance; 
        }
    }

    private void Awake()
    {
        CreateInstance();
    }

    private void CreateInstance()
    {
        if (!_instance)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void AddAnEvent(GameEnums.EEvents eventType, Action<object> function)
    {
        //Val là cái event, còn thg nào quan tâm cái event đó thì gọi hàm dưới
        _dictEvents.Add(eventType, function);
    }

    public void SubcribeAnEvent(GameEnums.EEvents eventType, Action<object> function)
    {
        _dictEvents[eventType] += function;
    }

    public void UnsubcribeAnEvent(GameEnums.EEvents eventType, Action<object> function)
    {
        _dictEvents[eventType] -= function;
    }

    public void InvokeAnEvent(GameEnums.EEvents eventType, object eventArgsType)
    {
        //Gọi thằng đã sub cái eventType với tham số eventArgsType
        //(tránh bị gọi tất cả func đã đki cùng 1 lúc)
        _dictEvents[eventType]?.Invoke(eventArgsType);
        /*if (eventType == GameEnums.EEvents.PlayerOnJumpPassive)
            Debug.Log("count: " + _dictEvents.Count);
        Debug.Log("eType, eArgsType: " + eventType + ", " + eventArgsType);*/
    }
}
