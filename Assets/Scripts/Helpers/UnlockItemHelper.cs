using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;
using static GameConstants;

public static class UnlockItemHelper
{
    public static bool HandleUnlockSkill(ESkills skillName)
    {
        bool isUnlockSuccess = true;
        string filePath = Application.dataPath + SKILLS_DATA_PATH;
        SkillsController sC = JSONDataHelper.LoadFromJSon<SkillsController>(filePath);

        //xem lại, hiện tại phải cần 1 file List Skill với toàn bộ skill đã có sẵn 
        //thì mới lưu đc
        //còn ko thì nó ko kiếm thấy skill name dẫn đến ko vào đc đoạn mã if (isUnlock)
        foreach (var skill in sC.skills)
        {
            if (skill.SkillName == skillName)
            {
                if (skill.IsUnlock)
                {
                    string content = "Purchase Failed,\n" + skillName.ToString() + " Already Unlock!";
                    NotificationParam param = new(content, true, false, null, null, null);
                    ShowNotificationHelper.ShowNotification(param);
                    return !isUnlockSuccess;
                }

                skill.IsUnlock = true;
                break;
            }
        }

        JSONDataHelper.SaveToJSon<SkillsController>(sC, filePath);

        return isUnlockSuccess;
    }
}
