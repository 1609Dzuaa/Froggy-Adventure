using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "ScriptableObjects/LevelStaticData")]
public class LevelStaticData : ScriptableObject
{
    public int OrderID;
    public Sprite ImageDescribe;
    public string Describe;
    [Tooltip("Tính bằng s")] public int TimeAllow;
}
