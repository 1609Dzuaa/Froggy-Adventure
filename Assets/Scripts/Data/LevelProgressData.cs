using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelProgressData
{
    public int LevelIndex;
    public bool IsUnlock;
    public bool IsCompleted;
    public int TimeCompleted;
    //public Vector3 PlayerPos;

    public LevelProgressData(int levelIndex, bool isUnlock, bool isCompleted, int timeCompleted) 
    {
        LevelIndex = levelIndex;
        IsUnlock = isUnlock;
        IsCompleted = isCompleted;
        TimeCompleted = timeCompleted;
    }
}
