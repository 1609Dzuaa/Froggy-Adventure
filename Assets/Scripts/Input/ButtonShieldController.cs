using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;
using static GameConstants;

public class ButtonShieldController : ButtonCooldownController, ISkillOnClick
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override List<Skills> GetListSkills(bool isLimited = false)
    {
        return base.GetListSkills(true);
    }

    public void OnClick()
    {
        if(!_isCooldown)
        {
            _isCooldown = true;
            EventsManager.Instance.NotifyObservers(EEvents.OnUseSkill, _btnSkill);
            HandleCooldown(BUTTON_BUFF_COOLDOWN_DURATION);
        }
    }

    protected override void DisplayText(string decimalType = "F1")
    {
        base.DisplayText(NO_DECIMAL_FORMAT);
    }
}
