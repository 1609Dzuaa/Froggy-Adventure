using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "ScriptableObjects/CoinData")]
public class CoinData : ScriptableObject
{
    public int MinValue;
    public int MaxValue;
    public float TweenDuration;
}
