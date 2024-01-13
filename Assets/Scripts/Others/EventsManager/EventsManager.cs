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
    //Thêm sẵn các Action tương ứng với Event trong EnumEvents tại đây
    private Action<object> EnemiesOnDamagePlayer;
    private Action<object> BulletOnHit;
    private Action<object> PlayerOnJumpPassive;
    private Action<object> PlayerOnInteractWithNPCs;

    public static EventsManager Instance
    {
        get 
        {
            if (!_instance)
                _instance = FindObjectOfType<EventsManager>();

            if (!_instance)
                Debug.Log("0 co EventsManager trong Scene");
            else
                Debug.Log("Find 2nd");

            return _instance; 
        }
    }

    private void Awake()
    {
        CreateInstance();
        AddEventsToDictionary();
    }

    private void CreateInstance()
    {
        if (!_instance)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("Khoi tao 1st");
        }
        else
            Destroy(gameObject);
    }

    public void AddEventsToDictionary()
    {
        _dictEvents.Add(GameEnums.EEvents.EnemiesOnDamagePlayer, EnemiesOnDamagePlayer);
        _dictEvents.Add(GameEnums.EEvents.BulletOnHit, BulletOnHit);
        _dictEvents.Add(GameEnums.EEvents.PlayerOnJumpPassive, PlayerOnJumpPassive);
        _dictEvents.Add(GameEnums.EEvents.PlayerOnInteractNPCs, PlayerOnInteractWithNPCs);
        //Val là cái event, còn thg nào quan tâm cái event đó thì gọi hàm dưới
    }

    public void SubcribeToAnEvent(GameEnums.EEvents eventType, Action<object> function)
    {
        _dictEvents[eventType] += function;
    }

    public void UnSubcribeToAnEvent(GameEnums.EEvents eventType, Action<object> function)
    {
        _dictEvents[eventType] -= function;
    }

    public void NotifyObservers(GameEnums.EEvents eventType, object eventArgsType)
    {
        //Gọi thằng đã sub cái eventType với tham số eventArgsType
        //(tránh bị gọi tất cả func đã đki cùng 1 lúc)
        _dictEvents[eventType]?.Invoke(eventArgsType);
    }
}
