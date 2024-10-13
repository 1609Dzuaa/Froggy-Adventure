using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameConstants;
using static GameEnums;

public class SkillData
{
    public ESkills Name;
    public float CooldownTime;

    public SkillData(ESkills name, float cooldownTime)
    {
        Name = name;
        CooldownTime = cooldownTime;
    }
}

public class ButtonSkillController : MonoBehaviour
{
    [SerializeField] protected ESkills _btnSkill; //btnDash, btnDbJump,...
    protected bool _isActivated;

    protected virtual void Awake()
    {
        string filePath = Application.dataPath + SKILLS_DATA_PATH;
        var list = ToggleAbilityItemHelper.GetListActivatedSkills(false);
        _isActivated = list.Find(x => x.SkillName == _btnSkill) != null;
    }
}
