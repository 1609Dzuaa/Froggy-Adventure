using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : BaseSingleton<UIManager>
{
    //Cần căn chỉnh pivot, anchors
    //Còn panel Level 1 - 2, loose Panel, win Panel

    [SerializeField] Animator _anim;
    [SerializeField] Canvas _sceneTransCanvas;

    GameObject _creditsPanel;
    GameObject _settingsPanel;
    bool _canPopUpPanel = true;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            PopUpSettingsPanel();
    }

    public void PopUpSettingsPanel()
    {
        if (!_canPopUpPanel) return;

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
        if (!_canPopUpPanel) return;

        _creditsPanel.SetActive(true);
       _settingsPanel.SetActive(false);
    }

    public void PopDownCreditsPanel()
    {
        _creditsPanel.SetActive(false);
    }

    public void IncreaseTransitionCanvasOrder()
    {
        _canPopUpPanel = false;
        _sceneTransCanvas.sortingOrder = 3;
    }

    public void DecreaseTransitionCanvasOrder()
    {
        _sceneTransCanvas.sortingOrder = -1;
        _canPopUpPanel = true;
    }

    public void TriggerAnimation(string para)
    {
        switch(para)
        {
            case GameConstants.SCENE_TRANS_END:
                _anim.SetTrigger(GameConstants.SCENE_TRANS_END);
                break;
            case GameConstants.SCENE_TRANS_START:
                _anim.SetTrigger(GameConstants.SCENE_TRANS_START);
                break;
        }
    }

}
