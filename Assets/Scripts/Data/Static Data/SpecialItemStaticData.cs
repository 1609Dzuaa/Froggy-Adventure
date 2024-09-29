using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

[System.Serializable]
public struct AbilityUI
{
    public ESkills AbilityName;
    public EFruits FruitName;
    [Header("Một vài skill sẽ giới hạn trong 1 level")] public bool IsLimited;
    public int FruitsRequired;
    [Header("Một số item sẽ có image skill != image item")] public Sprite AbilityImage;
    [Header("Mô tả chi tiết hơn khi mở khoá")] public string AbilityDescribe;
}

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "ScriptableObjects/SpecialItemStaticData")]
public class SpecialItemStaticData : ItemStaticData
{
    public AbilityUI Ability;
}
