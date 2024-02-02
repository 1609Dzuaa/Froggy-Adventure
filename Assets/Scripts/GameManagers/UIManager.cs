using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : BaseSingleton<UIManager>
{
    //Cần căn chỉnh pivot, anchors
    //Còn panel Level 1 - 2, loose Panel, win Panel

    GameObject _creditsPanel;
    GameObject _settingsPanel;

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
        //DontDestroyOnLoad(_settingsPanel);
        //DontDestroyOnLoad(_creditsPanel);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PopUpSettingsPanel();
        }
    }

    public void PopUpSettingsPanel()
    {
        _settingsPanel.SetActive(true);
        _creditsPanel.SetActive(false);
        if (SceneManager.GetActiveScene().buildIndex != 0)
            Time.timeScale = 0f;
    }

    public void PopDownSettingsPanel()
    {
        _settingsPanel.SetActive(false);
        if (SceneManager.GetActiveScene().buildIndex != 0)
            Time.timeScale = 1f;
    }

    public void PopUpCreditsPanel()
    {
       _creditsPanel.SetActive(true);
       _settingsPanel.SetActive(false);
    }

    public void PopDownCreditsPanel()
    {
        _creditsPanel.SetActive(false);
    }
}
