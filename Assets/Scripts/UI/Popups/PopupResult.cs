using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static GameEnums;
using static GameConstants;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultParam
{
    public ELevelResult Result;
    public int SilverCollected;
    public int GoldCollected;
    public int TimeCompleted;
    public int TimeAllow;

    public ResultParam(ELevelResult res, int silverCollected, int goldCollected, int timeComplete, int timeAllow)
    {
        Result = res;
        SilverCollected = silverCollected;
        GoldCollected = goldCollected;
        TimeCompleted = timeComplete;
        TimeAllow = timeAllow;
    }
}

public class PopupResult : PopupController
{
    [SerializeField] TextMeshProUGUI _txtBanner;
    [SerializeField] TextMeshProUGUI _txtSilver;
    [SerializeField] TextMeshProUGUI _txtGold;
    [SerializeField] TextMeshProUGUI _txtTime;
    [SerializeField] Image _imageBanner;
    [SerializeField] Sprite[] _spritesBanner;

    [Header("Tween related")]
    [SerializeField] float _distance;
    [SerializeField] float _tweenChildDuration;
    [SerializeField] Transform[] _arrButton;
    float _initPosition;
    float _endPosition;
    ResultParam _param;

    private void Awake()
    {
        _initPosition = transform.localPosition.y;
        _endPosition = transform.localPosition.y - _distance;
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnFinishLevel, ReceiveResultParam);
        ResetScaleButtons();
    }

    private void OnDestroy()
    {
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.OnFinishLevel, ReceiveResultParam);
    }

    private void ReceiveResultParam(object obj)
    {
        _param = obj as ResultParam;
        _txtBanner.text = (_param.Result == ELevelResult.Completed) ? "Level Completed" : "Level Failed";
        _imageBanner.sprite = (_param.Result == ELevelResult.Completed) ? _spritesBanner[0] : _spritesBanner[1];
        _txtSilver.text = "0";
        _txtGold.text = "0";
        TimeDisplayHelper.DisplayTime(ref _txtTime, _param.TimeCompleted, _param.TimeAllow);
    }

    protected override void OnEnable()
    {
        int startSCoin = 0;
        int startGCoin = 0;
        if (_isFirstOnEnable)
            _isFirstOnEnable = false;
        else
        {
            transform.DOLocalMoveY(_endPosition, _duration).SetEase(_ease).OnComplete(() =>
            {
                DOTween.To(() => startSCoin, x => startSCoin = x, _param.SilverCollected, _tweenChildDuration)
                .OnUpdate(() => { _txtSilver.text = startSCoin.ToString(); })
                .OnComplete(() =>
                {
                    DOTween.To(() => startGCoin, x => startGCoin = x, _param.GoldCollected, _tweenChildDuration)
                    .OnUpdate(() => { _txtGold.text = startGCoin.ToString(); })
                    .OnComplete(() =>
                    {
                        TweenButton();
                    });
                });
            }).SetUpdate(true);
        }
    }

    private void TweenButton()
    {
        Sequence sequence = DOTween.Sequence();
        foreach (Transform t in _arrButton)
            sequence.Append(t.DOScale(Vector3.one, _tweenChildDuration));
    }

    private void ResetScaleButtons()
    {
        foreach (Transform t in _arrButton)
            t.localScale = Vector3.zero;
    }

    protected override void OnDisable()
    {
        ResetScaleButtons();
    }

    public override void OnClose()
    {
        transform.DOLocalMoveY(_initPosition, _duration).SetEase(_ease)
                .OnComplete(() => { UIManager.Instance.TogglePopup(_popupName, false); });
    }

    public void ButtonOnClick(int buttonName)
    {
        switch (buttonName)
        {
            case (int)EButtonName.NextLevel:
                //check maxlevel here
                int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
                UIManager.Instance.AnimateAndTransitionScene(nextSceneIndex);
                break;

            case (int)EButtonName.Replay:
                int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
                UIManager.Instance.AnimateAndTransitionScene(currentSceneIndex);
                break;

            case (int)EButtonName.Home:
                UIManager.Instance.AnimateAndTransitionScene(GAME_MENU);
                break;

            case (int)EButtonName.Shop:
                UIManager.Instance.TogglePopup(EPopup.Shop, true);
                break;

            case (int)EButtonName.ChooseLevel:
                UIManager.Instance.TogglePopup(EPopup.Level, true);
                break;
        }
    }
}
