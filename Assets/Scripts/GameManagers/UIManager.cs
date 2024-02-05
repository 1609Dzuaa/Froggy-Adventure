using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameEnums;

public class UIManager : BaseSingleton<UIManager>
{
    //Cần căn chỉnh pivot, anchors
    //Còn panel Level 1 - 2, loose Panel, win Panel

    [SerializeField] Animator _anim;
    [SerializeField] Canvas _sceneTransCanvas;

    [SerializeField] GameObject _creditsPanel;
    [SerializeField] GameObject _settingsPanel;
    [SerializeField] GameObject _winPanel;
    [SerializeField] GameObject _loosePanel;
    [SerializeField] GameObject _hpsPanel;

    bool _canPopUpPanel = true;
    bool _canPlayCloseSfx = true;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _creditsPanel.SetActive(false);
        _settingsPanel.SetActive(false);
        _hpsPanel.SetActive(false);
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
        SoundsManager.Instance.PlaySfx(ESoundName.ButtonSelectedSfx, 1.0f);
        if (SceneManager.GetActiveScene().buildIndex != 0)
            Time.timeScale = 0f;
    }

    public void PopDownSettingsPanel()
    {
        _settingsPanel.SetActive(false);
        if (_canPlayCloseSfx)
            SoundsManager.Instance.PlaySfx(ESoundName.CloseButtonSfx, 1.0f);
        if (SceneManager.GetActiveScene().buildIndex != 0)
            Time.timeScale = 1f;
    }

    public void PopUpCreditsPanel()
    {
        if (!_canPopUpPanel) return;

        _creditsPanel.SetActive(true);
        _settingsPanel.SetActive(false);
        SoundsManager.Instance.PlaySfx(ESoundName.ButtonSelectedSfx, 1.0f);
    }

    public void PopDownCreditsPanel()
    {
        if (_canPlayCloseSfx)
            SoundsManager.Instance.PlaySfx(ESoundName.CloseButtonSfx, 1.0f);
        _creditsPanel.SetActive(false);
    }

    public void IncreaseTransitionCanvasOrder()
    {
        //Lúc này là đang chuyển Scene
        _canPopUpPanel = false;
        _sceneTransCanvas.sortingOrder = 3;
        _canPlayCloseSfx = false;
    }

    public void DecreaseTransitionCanvasOrder()
    {
        _sceneTransCanvas.sortingOrder = -1;
        _canPopUpPanel = true;
        _canPlayCloseSfx = true;
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

    public void PopUpHPCanvas()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
            _hpsPanel.SetActive(true);
    }

}
