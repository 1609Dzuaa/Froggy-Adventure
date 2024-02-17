using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameEnums;

[Serializable]
public struct UISkillUnlocked
{
    public EPlayerState _skillName;
    public Sprite _skillImage;
}

public class UIManager : BaseSingleton<UIManager>
{
    //Cần căn chỉnh pivot, anchors
    //Còn panel Level 1 - 2, loose Panel, win Panel

    [SerializeField] Animator _anim;
    [SerializeField] Canvas _sceneTransCanvas;

    [SerializeField] GameObject _startMenuCanvas;
    [SerializeField] GameObject _creditsPanel;
    [SerializeField] GameObject _settingsPanel;
    [SerializeField] GameObject _winPanel;
    [SerializeField] GameObject _loosePanel;
    [SerializeField] GameObject _hpsPanel;
    [SerializeField] GameObject _skillsAchievedPanel;
    [SerializeField] Image _skillsImage;
    [SerializeField] GameObject _txtSkillName;
    [SerializeField] List<UISkillUnlocked> _listSkill;

    [SerializeField] float _delayPopUpLoosePanel;
    [SerializeField] float _delayPopUpWinPanel;

    bool _canPopUpPanel = true;
    bool _canPlayCloseSfx; //Chỉ có thể play close sfx khi chủ động bấm X

    public GameObject StartMenuCanvas { get => _startMenuCanvas; set => _startMenuCanvas = value; }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        EventsManager.Instance.SubcribeToAnEvent(EEvents.PlayerOnUnlockSkills, PopUpSkillAchievedPanel);
        EventsManager.Instance.SubcribeToAnEvent(EEvents.PlayerOnWinGame, WinPanel);
    }

    private void OnDestroy()
    {
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.PlayerOnUnlockSkills, PopUpSkillAchievedPanel);
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.PlayerOnWinGame, WinPanel);
    }

    private void Start()
    {
        PopDownAllPanels();
    }

    private void Update()
    {
        //Chỉ cho bật Tab khi scene != start scene
        if (Input.GetKeyDown(KeyCode.Tab) && SceneManager.GetActiveScene().buildIndex != 0)
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

    private void PopDownSettingsPanel()
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

    private void PopDownCreditsPanel()
    {
        if (_canPlayCloseSfx)
            SoundsManager.Instance.PlaySfx(ESoundName.CloseButtonSfx, 1.0f);
        _creditsPanel.SetActive(false);
    }

    public IEnumerator PopUpLoosePanel()
    {
        _canPlayCloseSfx = false;
        PopDownAllPanels();

        yield return new WaitForSeconds(_delayPopUpLoosePanel);

        _loosePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void PopDownWinPanel()
    {
        _winPanel.SetActive(false);
    }

    private void PopDownLoosePanel()
    {
        _loosePanel.SetActive(false);
    }

    public void IncreaseTransitionCanvasOrder()
    {
        _canPopUpPanel = false;
        _sceneTransCanvas.sortingOrder = 3;
        _canPlayCloseSfx = false;
        //Lúc này là đang chuyển Scene
    }

    public void DecreaseTransitionCanvasOrder()
    {
        _sceneTransCanvas.sortingOrder = -1;
        _canPopUpPanel = true;
        _canPlayCloseSfx = true;
        //Lúc này là đã chuyển xong Scene
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
        //Thanh máu chỉ hiện ở các scene kh phải là Start Scene
    }

    public void PopDownHPCanvas()
    {
        _hpsPanel.SetActive(false);
    }

    public void PopDownAllPanels()
    {
        if (_canPlayCloseSfx)
            _canPlayCloseSfx = false;
        PopDownCreditsPanel();
        PopDownSettingsPanel();
        PopDownWinPanel();
        PopDownLoosePanel();
        PopDownHPCanvas();
        PopDownSkillAchievedPanel();
        _canPlayCloseSfx = true;
    }

    private void PopUpSkillAchievedPanel(object obj)
    {
        foreach (var item in _listSkill)
            if ((EPlayerState)obj == item._skillName)
            {
                _skillsImage.sprite = item._skillImage;
                _txtSkillName.GetComponent<TextMeshProUGUI>().text = item._skillName.ToString();
            }
        Time.timeScale = 0f;
        _skillsAchievedPanel.SetActive(true);
        SoundsManager.Instance.PlaySfx(ESoundName.SkillsAchivedSfx, 1.0f);
    }

    public void PopDownSkillAchievedPanel()
    {
        Time.timeScale = 1.0f;
        if (_canPlayCloseSfx)
            SoundsManager.Instance.PlaySfx(ESoundName.CloseButtonSfx, 1.0f);
        _skillsAchievedPanel.SetActive(false);
    }

    private void PopUpWinPanel()
    {
        PopDownAllPanels();
        _winPanel.SetActive(true);
    }

    private IEnumerator StartPopUpWinPanel()
    {
        yield return new WaitForSeconds(_delayPopUpWinPanel);

        PopUpWinPanel();
    }

    private void WinPanel(object obj)
    {
        StartCoroutine(StartPopUpWinPanel());
    }

}
