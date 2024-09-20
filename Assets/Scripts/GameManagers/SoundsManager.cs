using System;
using System.Collections;
using UnityEngine;
using static GameEnums;

public class SoundsManager : BaseSingleton<SoundsManager>
{
    //Clip - tệp âm thanh
    //Source - cái để phát tệp đó cũng như điều chỉnh linh tinh

    [SerializeField]
    Sounds[] sfxSounds, musicSounds;

    [SerializeField] AudioSource _sfxSource, _musicSource;
    [SerializeField] float _bgmusicDelay;

    bool _isPlayingBossTheme; //BossTheme sẽ có ngoại lệ != các Theme là nó sẽ tắt khi Replay

    public bool IsPlayingBossTheme { get => _isPlayingBossTheme; }

    public AudioSource SFXSource => _sfxSource;

    public AudioSource MusicSource => _musicSource;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PlayBackGroundMusic();
    }

    private void PlayBackGroundMusic()
    {
        PlayMusic(ESoundName.StartMenuTheme);
    }

    public void PlaySfx(ESoundName sfxName, float volumeScale)
    {
        //Tìm hiểu về Lambda expression
        Sounds s = Array.Find(sfxSounds, x => x.SoundName == sfxName);
        if (s == null)
            Debug.Log(sfxName + " Not Found");
        else
        {
            _sfxSource.clip = s.SoundAudioClip;
            if (volumeScale >= 1.0f) _sfxSource.PlayOneShot(_sfxSource.clip);
            else _sfxSource.PlayOneShot(_sfxSource.clip, volumeScale);
            //Debug.Log("Sfx Played: " + sfxName);
        }
    }

    public void PlayMusic(ESoundName musicName)
    {
        Sounds s = Array.Find(musicSounds, x => x.SoundName == musicName);
        if (s == null)
            Debug.Log(musicName + " Not Found");
        else
        {
            if (musicName == ESoundName.BossTheme) 
                _isPlayingBossTheme = true;
            else 
                _isPlayingBossTheme = false;
            _musicSource.clip = s.SoundAudioClip;
            _musicSource.PlayDelayed(_bgmusicDelay);
        }
    }

    public void ChangeMusicVolume(float para)
    {
        _musicSource.volume = para;
    }

    public void ChangeSfxVolume(float para)
    {
        _sfxSource.volume = para;
    }
}
