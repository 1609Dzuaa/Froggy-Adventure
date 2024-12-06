using System;
using System.Collections;
using UnityEngine;
using static GameEnums;
using static GameConstants;
using UnityEngine.UI;

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

    public void LoadVolumeConfigs(Slider musicSlider, Slider sfxSlider)
    {
        if (PlayerPrefs.HasKey(MUSIC_CONFIG))
        {
            _musicSource.volume = PlayerPrefs.GetFloat(MUSIC_CONFIG);
            musicSlider.value = PlayerPrefs.GetFloat(MUSIC_CONFIG);
        }
        else
        {
            _musicSource.volume = DEFAULT_VOLUME;
            musicSlider.value = DEFAULT_VOLUME;
        }

        if (PlayerPrefs.HasKey(SFX_CONFIG))
        {
            _sfxSource.volume = PlayerPrefs.GetFloat(SFX_CONFIG);
            sfxSlider.value = PlayerPrefs.GetFloat(SFX_CONFIG);
        }
        else
        {
            _musicSource.volume = DEFAULT_VOLUME;
            sfxSlider.value = DEFAULT_VOLUME;
        }
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

    public void ConfigVolume(string key, float value)
    {
        if (key == MUSIC_CONFIG)
            _musicSource.volume = value;
        else
            _sfxSource.volume = value;
    }
}
