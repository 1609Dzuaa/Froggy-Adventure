using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;
using static GameConstants;

public static class ToggleAbilityItemHelper
{
    public static bool ToggleLockSkill(ESkills skillName, bool isLock)
    {
        bool isProcessSuccess = true;
        string filePath = Application.dataPath + SKILLS_DATA_PATH;
        SkillsController sC = JSONDataHelper.LoadFromJSon<SkillsController>(filePath);

        if (!isLock)
        {
            foreach (var skill in sC.skills)
            {
                if (skill.SkillName == skillName)
                {
                    if (skill.IsUnlock)
                    {
                        string content = "Purchase Failed,\n" + skillName.ToString() + " Already Unlock!";
                        NotificationParam param = new(content, true, false, null, null, null);
                        ShowNotificationHelper.ShowNotification(param);
                        return !isProcessSuccess;
                    }

                    skill.IsUnlock = true;
                    break;
                }
            }
        }
        else
        {
            foreach (var s in sC.skills)
            {
                if (skillName == s.SkillName)
                {
                    s.IsUnlock = false;
                    break;
                }
            }
        }

        JSONDataHelper.SaveToJSon<SkillsController>(sC, filePath);
        return isProcessSuccess;
    }

    /// <summary>
    /// func này trả về list các skill activated dựa vào param
    /// </summary>
    /// <param name="isLimited">check xem skill này có phải skill giới hạn kh</param>
    /// <returns></returns>
    public static List<Skills> GetListActivatedSkills(bool isLimited = true)
    {
        string filePath = Application.dataPath + SKILLS_DATA_PATH;
        SkillsController sC = JSONDataHelper.LoadFromJSon<SkillsController>(filePath);
        var list = new List<Skills>();

        if (isLimited)
        {
            foreach (var s in sC.skills)
                if (s.IsUnlock == true && s.IsLimited)
                    list.Add(s);
        }
        else 
        {
            foreach (var s in sC.skills)
                if (s.IsUnlock == true && !s.IsLimited)
                    list.Add(s);
        }

        return list;
    }
}
