using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public class BuffsManager : BaseSingleton<BuffsManager>
{
    [SerializeField] PlayerInvisibleBuff _invisibleBuff;
    [SerializeField] PlayerShieldBuff _shieldBuff;

    private Dictionary<EBuffs, PlayerBuffs> _dictBuffs = new();

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        InitBuffDictionary();
        foreach (var buff in _dictBuffs.Values)
            buff.Start();
    }

    /*private void Update()
    {
        foreach(var buff  in _dictBuffs.Values)
            buff.Update();
    }*/

    private void InitBuffDictionary()
    {
        _dictBuffs.Add(EBuffs.Invisible, _invisibleBuff);
        _dictBuffs.Add(EBuffs.Shield, _shieldBuff);
    }

    public PlayerBuffs GetBuff(EBuffs buffType)
    {
        return _dictBuffs[buffType];
    }
}
