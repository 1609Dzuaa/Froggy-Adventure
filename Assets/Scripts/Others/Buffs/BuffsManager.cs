using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffsManager : MonoBehaviour
{
    private static BuffsManager _buffsManagerInstance;
    private Dictionary<GameEnums.EBuffs, PlayerBuffs> _dictBuffs = new();

    public Action<object> ApplyBuff;

    public static BuffsManager Instance
    {
        get
        {
            if (!_buffsManagerInstance)
                FindObjectOfType<BuffsManager>();

            if (!_buffsManagerInstance)
                Debug.Log("0 co BuffsManager trong Scene");

            return _buffsManagerInstance;
        }
    }

    private void Awake()
    {
        CreateInstance();
        InitBuffDictionary();
    }

    private void Start()
    {
        //EventsManager.Instance.AddAnEvent(GameEnums.EEvents.PlayerOnAbsorbBuffs, ApplyBuff);
        foreach (var buff in _dictBuffs.Values)
            buff.Start();
    }

    private void Update()
    {
        foreach(var buff  in _dictBuffs.Values)
            buff.Update();
    }

    private void CreateInstance()
    {
        if (!_buffsManagerInstance)
        {
            _buffsManagerInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
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
