using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public class EventsManager : BaseSingleton<EventsManager>
{
    //You can assign values of any type to variables of type object
    //https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/reference-types
    private Dictionary<EEvents, Action<object>> _dictEvents = new();
    //Thêm sẵn các Action tương ứng với Event trong EnumEvents tại đây
    private readonly Action<object> PlayerOnTakeDamage;
    private readonly Action<object> BulletOnHit;
    private readonly Action<object> BulletOnReceiveInfo;
    private readonly Action<object> PlayerOnJumpPassive;
    private readonly Action<object> PlayerOnInteractWithNPCs;
    private readonly Action<object> PlayerOnStopInteractWithNPCs;
    private readonly Action<object> PlayerOnBeingPushedBack;
    private readonly Action<object> PlayerOnUpdateRespawnPosition;
    private readonly Action<object> PlayerOnWinGame;
    private readonly Action<object> FanOnBeingDisabled;
    private readonly Action<object> ObjectOnRestart;
    private readonly Action<object> TutorOnDestroy;
    private readonly Action<object> OnUnlockSkill;
    private readonly Action<object> CameraOnShake;
    private readonly Action<object> BossOnSummonMinion;
    private readonly Action<object> BossGateOnClose;
    private readonly Action<object> BossOnDie;

    protected override void Awake()
    {
        base.Awake();
        AddEventsToDictionary();
        DontDestroyOnLoad(gameObject);
    }

    public void AddEventsToDictionary()
    {
        _dictEvents.Add(EEvents.PlayerOnTakeDamage, PlayerOnTakeDamage);
        _dictEvents.Add(EEvents.BulletOnHit, BulletOnHit);
        _dictEvents.Add(EEvents.BulletOnReceiveInfo, BulletOnReceiveInfo);
        _dictEvents.Add(EEvents.PlayerOnJumpPassive, PlayerOnJumpPassive);
        _dictEvents.Add(EEvents.PlayerOnInteractWithNPCs, PlayerOnInteractWithNPCs);
        _dictEvents.Add(EEvents.PlayerOnStopInteractWithNPCs, PlayerOnStopInteractWithNPCs); 
        _dictEvents.Add(EEvents.PlayerOnBeingPushedBack, PlayerOnBeingPushedBack);
        _dictEvents.Add(EEvents.PlayerOnUpdateRespawnPosition, PlayerOnUpdateRespawnPosition);
        _dictEvents.Add(EEvents.PlayerOnWinGame, PlayerOnWinGame);
        _dictEvents.Add(EEvents.FanOnBeingDisabled, FanOnBeingDisabled);
        _dictEvents.Add(EEvents.ObjectOnRestart, ObjectOnRestart);
        _dictEvents.Add(EEvents.TutorOnDestroy, TutorOnDestroy);
        //_dictEvents.Add(EEvents.OnUnlockSkill, OnUnlockSkill);
        _dictEvents.Add(EEvents.CameraOnShake, CameraOnShake);
        _dictEvents.Add(EEvents.BossOnSummonMinion, BossOnSummonMinion);
        _dictEvents.Add(EEvents.BossGateOnClose, BossGateOnClose);
        _dictEvents.Add(EEvents.BossOnDie, BossOnDie);
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

    public void UnSubcribeToAnEvent(EEvents eventName, Action<object> callback)
    {
        _dictEvents[eventName] -= callback;
    }

    public void NotifyObservers(EEvents eventName, object eventArgsType = null)
    {
        _dictEvents[eventName]?.Invoke(eventArgsType);
    }
}
