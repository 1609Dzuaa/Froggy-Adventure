using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    private static SoundsManager _smInstance;
    private Dictionary<string, AudioSource> _dictSounds = new();

    [Header("Player's Sound")]
    [SerializeField] private AudioSource _collectSound;
    [SerializeField] private AudioSource _jumpSound;
    [SerializeField] private AudioSource _collectHPSound;
    [SerializeField] private AudioSource _gotHitSound;
    [SerializeField] private AudioSource _deadSound;
    [SerializeField] private AudioSource _dashSound;

    [Header("Enemies's Sound")]
    [SerializeField] private AudioSource _plantShootSound;
    [SerializeField] private AudioSource _trunkShootSound;
    [SerializeField] private AudioSource _enemiesDeadSound;

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

        _dictSounds.Add(GameConstants.PLANT_SHOOT_SOUND, _plantShootSound);
        _dictSounds.Add(GameConstants.TRUNK_SHOOT_SOUND, _trunkShootSound);
        _dictSounds.Add(GameConstants.ENEMIES_DEAD_SOUND, _enemiesDeadSound);
    }

    public AudioSource GetTypeOfSound(string soundType)
    {
        return _dictSounds[soundType];
    }
}
