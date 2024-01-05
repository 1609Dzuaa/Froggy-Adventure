using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    private static SoundsManager _smInstance;
    private Dictionary<string, AudioSource> _dictSounds = new();

    [Header("Collect")]
    [SerializeField] private AudioSource _collectSound;

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
    }

    public AudioSource GetTypeOfSound(string soundType)
    {
        return _dictSounds[soundType];
    }
}
