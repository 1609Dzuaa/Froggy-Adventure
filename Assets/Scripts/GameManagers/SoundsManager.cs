﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
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

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        StartCoroutine(PlayBackGroundMusic());
    }

    private IEnumerator PlayBackGroundMusic()
    {
        yield return new WaitForSeconds(_bgmusicDelay);

        PlayMusic(ESoundName.Theme);
    }

    public void PlaySfx(ESoundName sfxName)
    {
        //Tìm hiểu về Lambda expression
        Sounds s = Array.Find(sfxSounds, x => x.SoundName == sfxName);
        if (s == null)
            Debug.Log(sfxName + " Not Found");
        else
        {
            _sfxSource.clip = s.SoundAudioClip;
            _sfxSource.PlayOneShot(_sfxSource.clip);
        }
    }

    public void PlayMusic(ESoundName musicName)
    {
        Sounds s = Array.Find(musicSounds, x => x.SoundName == musicName);
        if (s == null)
            Debug.Log(musicName + " Not Found");
        else
        {
            _musicSource.clip = s.SoundAudioClip;
            _musicSource.Play();
        }
    }
}
