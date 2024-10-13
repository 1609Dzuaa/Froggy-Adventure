using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public class ButtonShieldController : ButtonSkillController, ISkillOnClick
{
    protected override void Awake()
    {
        base.Awake();
        gameObject.SetActive(_isActivated);
    }

    public void OnClick()
    {
        BuffsManager.Instance.GetTypeOfBuff(EBuffs.Shield).ApplyBuff();
        EventsManager.Instance.NotifyObservers(EEvents.OnUseSkill, _btnSkill);
    }
}
