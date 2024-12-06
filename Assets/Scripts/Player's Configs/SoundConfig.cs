using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameConstants;

public class SoundConfig : MonoBehaviour
{
    [SerializeField] Slider[] _arrSliders; //0 la Music, 1 la Sfx

    const int INDEX_MUSIC_SLIDER = 0;
    const int INDEX_SFX_SLIDER = 1;

    private void Awake()
    {
        SoundsManager.Instance.LoadVolumeConfigs(_arrSliders[INDEX_MUSIC_SLIDER], _arrSliders[INDEX_SFX_SLIDER]);
    }

    public void OnValueChanged(int index)
    {
        if (index == INDEX_MUSIC_SLIDER)
        {
            PlayerPrefs.SetFloat(MUSIC_CONFIG, _arrSliders[0].value);
            SoundsManager.Instance.ConfigVolume(MUSIC_CONFIG, _arrSliders[0].value);
        }
        else
        {
            PlayerPrefs.SetFloat(SFX_CONFIG, _arrSliders[1].value);
            SoundsManager.Instance.ConfigVolume(SFX_CONFIG, _arrSliders[1].value);
        }
    }
}
