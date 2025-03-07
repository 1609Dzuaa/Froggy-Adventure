using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public class PlayerActiveBuffs : PlayerBuffs
{
    public override void Awake()
    {
        base.Awake();
        EventsManager.SubcribeToAnEvent(EEvents.OnUseSkill, PerformSkill);
    }

    protected void OnDestroy()
    {
        EventsManager.UnsubscribeToAnEvent(EEvents.OnUseSkill, PerformSkill);
    }

    protected void PerformSkill(object obj) 
    {
        ESkills name = (ESkills)obj;
        if (name == ESkills.Shield && !_isActivating)
        {
            _isActivating = true;
            HandleActiveBuff();
        }
    }

    protected virtual void HandleActiveBuff() { }
}
