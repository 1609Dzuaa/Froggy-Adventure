using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "ScriptableObjects/FruitData")]
public class FruitData : ScriptableObject
{
    public EFruits FruitName;
    public Sprite FruitImage;
}
