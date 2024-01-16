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
    private Action<object> PlayerOnTakeDamage;
    private Action<object> BulletOnHit;
    private Action<object> BulletOnReceiveInfo;
    private Action<object> PlayerOnJumpPassive;
    private Action<object> PlayerOnInteractWithNPCs;
    private Action<object> PlayerOnStopInteractWithNPCs;

    //Làm việc với Event thì nên phân biệt với nhau bằng key là object
    //Tránh cùng 1 lúc nó Notify tất cả Func đã đky event đó
    //thay vì chỉ Notify những Func cần

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
        AddEventsToDictionary();
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

    public void AddEventsToDictionary()
    {
        _dictEvents.Add(GameEnums.EEvents.PlayerOnTakeDamage, PlayerOnTakeDamage);
        _dictEvents.Add(GameEnums.EEvents.BulletOnHit, BulletOnHit);
        _dictEvents.Add(GameEnums.EEvents.BulletOnReceiveInfo, BulletOnReceiveInfo);
        _dictEvents.Add(GameEnums.EEvents.PlayerOnJumpPassive, PlayerOnJumpPassive);
        _dictEvents.Add(GameEnums.EEvents.PlayerOnInteractWithNPCs, PlayerOnInteractWithNPCs);
        _dictEvents.Add(GameEnums.EEvents.PlayerOnStopInteractWithNPCs, PlayerOnStopInteractWithNPCs);
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
