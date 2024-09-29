using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

[System.Serializable]
public class PlayerData
{
    public int HealthPoint;
    public int MaxHealthPoint;
    public int SilverCoin;
    public int GoldCoin;

    public PlayerData(int hp, int maxHP, int silv, int gold)
    {
        HealthPoint = hp;
        MaxHealthPoint = maxHP;
        SilverCoin = silv;
        GoldCoin = gold;
    }
}

[System.Serializable]
public class Skills
{
    public ESkills SkillName;
    public bool IsUnlock;
    public bool IsLimited;

    public Skills(ESkills skillName, bool isUnlock, bool isLimited)
    {
        SkillName = skillName;
        IsUnlock = isUnlock;
        IsLimited = isLimited;
    }
}

[System.Serializable]
public class SkillsController
{
    public List<Skills> skills;

    public SkillsController(List<Skills> skills)
    {
        this.skills = skills;
    }
}

[System.Serializable]
public class Fruits
{
    public EFruits FruitName;
    public int FruitCount;

    public Fruits(EFruits fruitName, int fruitCount)
    {
        FruitName = fruitName;
        FruitCount = fruitCount;
    }
}

[System.Serializable]
public class FruitsIventory
{
    public List<Fruits> Fruits;

    public FruitsIventory(List<Fruits> fruits)
    {
        this.Fruits = fruits;
    }
}