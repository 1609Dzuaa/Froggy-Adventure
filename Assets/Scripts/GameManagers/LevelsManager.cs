using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManager : BaseSingleton<LevelsManager>
{
    public Dictionary<int, LevelProgressData> DictLevelsProgress;
    public Dictionary<int, LevelStaticData> DictLevelsStaticData;

    protected override void Awake()
    {
        base.Awake();
        DictLevelsStaticData = new();
        DictLevelsProgress = new();
        DontDestroyOnLoad(gameObject);
    }
}
