using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : BaseSingleton<UIManager>
{
    //Cần căn chỉnh pivot, anchors

    GameObject _creditsPanel;
    GameObject _settingsPanel;
    bool _isCreditsActive;
    bool _isSettingsActive;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _creditsPanel = GameObject.FindWithTag(GameConstants.CREDITS_TAG);
        _settingsPanel = GameObject.FindWithTag(GameConstants.SETTINGS_TAG);
        _creditsPanel.SetActive(false);
        _settingsPanel.SetActive(false);
    }

    public void PopUpSettingsPanel()
    {
        if(!_isSettingsActive)
        {
            _isSettingsActive = true;
            _isCreditsActive = false;
            _settingsPanel.SetActive(true);
            _creditsPanel.SetActive(false);
        }
        else
        {
            _isSettingsActive = false;
            _settingsPanel.SetActive(false);
        }
    }

    public void PopUpCreditsPanel()
    {
        if (!_isCreditsActive)
        {
            _isCreditsActive = true;
            _isSettingsActive = false;
            _creditsPanel.SetActive(true);
            _settingsPanel.SetActive(false);
        }
        else
        {
            _isCreditsActive = false;
            _creditsPanel.SetActive(false);
        }
    }
}
