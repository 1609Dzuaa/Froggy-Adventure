using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManager : BaseSingleton<LevelsManager>
{
    public Dictionary<int, LevelProgressData> DictLevelsProgress;

    protected override void Awake()
    {
        base.Awake();
        DictLevelsProgress = new();
        DontDestroyOnLoad(gameObject);
    }
}
