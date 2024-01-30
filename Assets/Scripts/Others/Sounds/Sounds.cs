using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

//Class này để tạo mảng các Sound đi kèm với AudioClip của từng Sound để Play
[System.Serializable] //Dùng cho Class
public class Sounds
{
    [SerializeField] ESoundName _soundName;

    //Nhận vào audioClip, đỡ phải tạo AS
    [SerializeField] AudioClip _soundAudioClip;

    public ESoundName SoundName { get { return _soundName; } }

    public AudioClip SoundAudioClip { get { return _soundAudioClip; } }
}