using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;
using static GameConstants;

public static class InitFileHelper
{
    public static void InitAbilityAndFruitFiles(ItemShop[] arrParam)
    {
        PlayerPrefs.DeleteAll();
        if (!PlayerPrefs.HasKey(FRUIT_AND_SKILL_CREATED))
        {
            List<Skills> listSkills = new();
            List<Fruits> listFruits = new();
            HashSet<ESkills> hashSkill = new();
            HashSet<EFruits> hashFruits = new();

            //Fill in List Skill và Fruit
            foreach (var item in arrParam)
            {
                if (item is AbilityItemShop)
                {
                    var ability = (AbilityItemShop)item;

                    //add skill vào list skills
                    if (!hashSkill.Contains(ability.SISData.Ability.AbilityName))
                    {
                        var name = ability.SISData.Ability.AbilityName;
                        var isLimited = ability.SISData.Ability.IsLimited;
                        hashSkill.Add(name);
                        Skills sk = new(name, DEFAULT_UNLOCK_ITEM, isLimited);
                        listSkills.Add(sk);
                    }

                    //add fruit vào list fruits
                    if (!hashFruits.Contains(ability.SISData.Ability.FruitName))
                    {
                        var name = ability.SISData.Ability.FruitName;
                        hashFruits.Add(name);
                        Fruits fr = new(name, DEFAULT_ITEM_COUNT);
                        listFruits.Add(fr);
                    }
                }
            }

            string skillsFilePath = Application.dataPath + SKILLS_DATA_PATH;
            SkillsController sC = new(listSkills);
            JSONDataHelper.SaveToJSon<SkillsController>(sC, skillsFilePath);

            string fruitsFilePath = Application.dataPath + FRUITS_DATA_PATH;
            FruitsIventory fI = new(listFruits);
            JSONDataHelper.SaveToJSon<FruitsIventory>(fI, fruitsFilePath);

            PlayerPrefs.SetInt(FRUIT_AND_SKILL_CREATED, CREATED);
        }
    }
}
