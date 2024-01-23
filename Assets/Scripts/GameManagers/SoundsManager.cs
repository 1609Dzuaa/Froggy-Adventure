﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    //Vấn đề của SM hiện tại:
    //Vật thể nào đó vẫn có thể phát sound dù cách rất xa Player 

    private static SoundsManager _smInstance;
    private Dictionary<string, AudioSource> _dictSounds = new();

    [Header("Player's Sound")]
    [SerializeField] private AudioSource _collectSound;
    [SerializeField] private AudioSource _jumpSound;
    [SerializeField] private AudioSource _collectHPSound;
    [SerializeField] private AudioSource _gotHitSound;
    [SerializeField] private AudioSource _deadSound;
    [SerializeField] private AudioSource _dashSound;
    [SerializeField] private AudioSource _landSound;

    [Header("Enemies's Sound")]
    [SerializeField] private AudioSource _plantShootSound;
    [SerializeField] private AudioSource _trunkShootSound;
    [SerializeField] private AudioSource _enemiesDeadSound;

    [Header("Gecko's Sound")]
    [SerializeField] private AudioSource _geckoAttackSound;

    public static SoundsManager Instance
    {
        get
        {
            if (!_smInstance)
            {
                _smInstance = FindObjectOfType<SoundsManager>();
                if (!_smInstance)
                    Debug.Log("0 co SoundsManager trong Scene");
            }

            return _smInstance;
        }
    }

    private void Awake()
    {
        CreateInstance();
        InitSoundDictionary();
    }

    private void CreateInstance()
    {
        if (!_smInstance)
        {
            _smInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void InitSoundDictionary()
    {
        _dictSounds.Add(GameConstants.COLLECT_FRUITS_SOUND, _collectSound);
        _dictSounds.Add(GameConstants.COLLECT_HP_SOUND, _collectHPSound);
        _dictSounds.Add(GameConstants.PLAYER_DASH_SOUND, _dashSound);
        _dictSounds.Add(GameConstants.PLAYER_DEAD_SOUND, _deadSound);
        _dictSounds.Add(GameConstants.PLAYER_GOT_HIT_SOUND, _gotHitSound);
        _dictSounds.Add(GameConstants.PLAYER_JUMP_SOUND, _jumpSound);
        _dictSounds.Add(GameConstants.PLAYER_LAND_SOUND, _landSound);

        _dictSounds.Add(GameConstants.PLANT_SHOOT_SOUND, _plantShootSound);
        _dictSounds.Add(GameConstants.TRUNK_SHOOT_SOUND, _trunkShootSound);
        _dictSounds.Add(GameConstants.ENEMIES_DEAD_SOUND, _enemiesDeadSound);

        _dictSounds.Add(GameConstants.GECKO_ATTACK_SOUND, _geckoAttackSound);
    }

    public AudioSource GetTypeOfSound(string soundType)
    {
        return _dictSounds[soundType];
    }
}