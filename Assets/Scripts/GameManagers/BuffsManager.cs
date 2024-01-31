using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffsManager : BaseSingleton<BuffsManager>
{
    private Dictionary<GameEnums.EBuffs, PlayerBuffs> _dictBuffs = new();

    protected override void Awake()
    {
        base.Awake();
        InitBuffDictionary();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        foreach (var buff in _dictBuffs.Values)
            buff.Start();
    }

    private void Update()
    {
        foreach(var buff  in _dictBuffs.Values)
            buff.Update();
    }

    private void InitBuffDictionary()
    {
        _dictBuffs.Add(GameEnums.EBuffs.Speed, FindObjectOfType<PlayerSpeedBuff>());
        _dictBuffs.Add(GameEnums.EBuffs.Jump, FindObjectOfType<PlayerJumpBuff>());
        _dictBuffs.Add(GameEnums.EBuffs.Invisible, FindObjectOfType<PlayerInvisibleBuff>());
        _dictBuffs.Add(GameEnums.EBuffs.Shield, FindObjectOfType<PlayerShieldBuff>());
        _dictBuffs.Add(GameEnums.EBuffs.Absorb, FindObjectOfType<PlayerAbsorbBuff>());
    }

    public PlayerBuffs GetTypeOfBuff(GameEnums.EBuffs buffType)
    {
        return _dictBuffs[buffType];
    }
}
