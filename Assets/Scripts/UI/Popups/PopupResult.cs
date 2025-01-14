using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static GameEnums;
using static GameConstants;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Linq;

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

    [Header("Để ý gắn thứ tự 1 2 3 cho đúng")]
    [SerializeField] TextMeshProUGUI[] _arrTxtFruit;
    [SerializeField] Image[] _arrImgFruit;
    [SerializeField] TextMeshProUGUI[] _arrTextTime;
    [SerializeField] Image _imageBanner;
    [SerializeField] Sprite[] _spritesBanner;
    [SerializeField] PlayerBagController _playerBag;

    [Header("Tween related")]
    [SerializeField] float _distance;
    [SerializeField] float _tweenChildDuration;
    [SerializeField] Transform[] _arrButton;

    //[SerializeField] PopupNotification _notification;
    float _initPosition;
    float _endPosition;
    bool _canClick = true; //prevent mutliple click
    bool _canClose = true; //prevent close this popup when the tasks havent's finished yet
    ResultParam _param;
    int _currentHP;
    int _currentSCoin;
    int _currentLevel;
    int _nextLevel;

    private void Awake()
    {
        _initPosition = transform.localPosition.y;
        _endPosition = transform.localPosition.y - _distance;
        EventsManager.SubcribeToAnEvent(EEvents.OnHandleLevelCompleted, ReceiveResultParam);
        ResetScaleButtons();
    }

    private void OnDestroy()
    {
        EventsManager.UnsubscribeToAnEvent(EEvents.OnHandleLevelCompleted, ReceiveResultParam);
    }

    private void ReceiveResultParam(object obj)
    {
        if (obj == null) return;
        _param = obj as ResultParam;
        _txtBanner.text = (_param.Result == ELevelResult.Completed) ? "Level Completed" : "Level Failed";
        _imageBanner.sprite = (_param.Result == ELevelResult.Completed) ? _spritesBanner[0] : _spritesBanner[1];
        _txtSilver.text = "0";
        _txtGold.text = "0";
        TimeDisplayHelper.DisplayTime(ref _txtTime, 0, _param.TimeAllow); //setup timer

        _currentHP = PlayerHealthManager.Instance.CurrentHP;
        _currentSCoin = PlayerDataController.Instance.PlayerBag.SilverCoin;
        _currentLevel = SceneManager.GetActiveScene().buildIndex;
        _nextLevel = _currentLevel + 1;

        //save data here
        PlayerDataController.Instance.PlayerBag.SilverCoin += _param.SilverCollected;
        PlayerDataController.Instance.PlayerBag.GoldCoin += _param.GoldCollected;
        EventsManager.NotifyObservers(EEvents.OnSavePlayerData);
    }

    //Vì có thể player đã mua đồ nhưng đống current data của player ch đc update
    private void GetCurrentPlayerData()
    {
        _currentHP = PlayerHealthManager.Instance.CurrentHP;
        _currentSCoin = PlayerDataController.Instance.PlayerBag.SilverCoin;
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
                //tween sCoin
                DOTween.To(() => startSCoin, x => startSCoin = x, _param.SilverCollected, _tweenChildDuration)
                .OnUpdate(() => { _txtSilver.text = startSCoin.ToString(); });

                //tween gCoin
                DOTween.To(() => startGCoin, x => startGCoin = x, _param.GoldCollected, _tweenChildDuration)
                    .OnUpdate(() => { _txtGold.text = startGCoin.ToString(); });

                //assign fruit's image & tween fruit's count
                var arrayFruitCollected = _playerBag.DictFruitsCollected.ToArray();
                for (int i = 0; i < arrayFruitCollected.Length; i++)
                {
                    //sử dụng biến tạm vì nếu sd biến i trong callback OnUpdate
                    //thì i có thể bị thay đổi trong quá trình update
                    int localIndex = i;
                    int startFruit = 0;
                    _arrImgFruit[localIndex].sprite = arrayFruitCollected[localIndex].Value.FruitImage;
                    DOTween.To(() => startFruit, x => startFruit = x, arrayFruitCollected[localIndex].Value.Count, _tweenChildDuration)
                .OnUpdate(() => { _arrTxtFruit[localIndex].text = startFruit.ToString(); });
                }

                //tween 3 thằng fruit count
                FadeImageFruits(1.0f);
                FadeTextFruits(1.0f, TweenTextTimerCallback);
            }).SetUpdate(true);
        }
    }

    private void TweenTextTimerCallback()
    {
        //tween text timer
        int startTimer = 0;
        _arrTextTime[0].DOFade(1.0f, _tweenChildDuration);
        _arrTextTime[1].DOFade(1.0f, _tweenChildDuration).OnComplete(() =>
        {
            DOTween.To(() => startTimer, x => startTimer = x, _param.TimeCompleted, _tweenChildDuration)
            .OnUpdate(() =>
            {
                TimeDisplayHelper.DisplayTime(ref _arrTextTime[1], startTimer, _param.TimeAllow);
            }).OnComplete(() =>
            {
                TweenButton();
            });
        });
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
        FadeTextFruits(0f);
        FadeImageFruits(0f);
        FadeTextTimer(0f);
    }

    private void FadeImageFruits(float value)
    {
        for (int i = 0; i < _arrImgFruit.Length; i++)
            _arrImgFruit[i].DOFade(value, _duration);
    }

    private void FadeTextFruits(float value, TweenCallback callback = null)
    {
        bool callbackCalled = false;
        for (int i = 0; i < _arrTxtFruit.Length; i++)
        {
            if (!callbackCalled && callback != null)
            {
                callbackCalled = true;
                _arrTxtFruit[i].DOFade(value, _tweenChildDuration).OnComplete(callback);
            }
            else
                _arrTxtFruit[i].DOFade(value, _tweenChildDuration);
        }
    }

    private void FadeTextTimer(float value)
    {
        for (int i = 0; i < _arrTextTime.Length; i++)
            _arrTextTime[i].DOFade(value, _duration);
    }

    public override void OnClose()
    {
        if (_canClose)
            transform.DOLocalMoveY(_initPosition, _duration).SetEase(_ease)
                    .OnComplete(() => { UIManager.Instance.TogglePopup(_popupName, false); });
    }

    private IEnumerator CooldownButton()
    {
        yield return new WaitForSeconds(COOLDOWN_BUTTON);

        _canClick = true;
    }

    public void ButtonOnClick(int buttonName)
    {

        switch (buttonName)
        {
            case (int)EButtonName.NextLevel:
                GetCurrentPlayerData();
                ProcessBasedOnPlayerHP(_currentHP, _currentSCoin, _nextLevel);
                break;

            case (int)EButtonName.Replay:
                GetCurrentPlayerData();
                ProcessBasedOnPlayerHP(_currentHP, _currentSCoin, _currentLevel);
                break;

            case (int)EButtonName.Home:
                if (_canClick)
                {
                    _canClick = false;
                    StartCoroutine(CooldownButton());
                    UIManager.Instance.AnimateAndTransitionScene(GAME_MENU, true, false, false, true);
                    EventsManager.NotifyObservers(EEvents.OnReturnMainMenu);
                }
                break;

            case (int)EButtonName.Shop:
                if (_canClick)
                {
                    _canClick = false;
                    StartCoroutine(CooldownButton());
                    UIManager.Instance.TogglePopup(EPopup.Shop, true);
                }
                break;

            case (int)EButtonName.ChooseLevel:
                if (_canClick) 
                {
                    _canClick = false;
                    StartCoroutine(CooldownButton());
                    UIManager.Instance.TogglePopup(EPopup.Level, true);
                }
                break;
        }
    }

    private void ShowShop()
    {
        UIManager.Instance.TogglePopup(EPopup.Notification, false);
        UIManager.Instance.TogglePopup(EPopup.Shop, true);
    }

    private void ProcessBasedOnPlayerHP(int hp, int silverCoin, int levelToSwitch, Action<int> callback = null)
    {
        _canClose = false;

        if (levelToSwitch > MAX_GAME_LEVEL)
        {
            string content = "Max Level Reached!";
            NotificationParam param = new(content, true, () =>
            {
                _canClick = false;
                StartCoroutine(CooldownButton());
            });
            ShowNotificationHelper.ShowNotification(param);
        }
        else if (_param.Result == ELevelResult.Failed && levelToSwitch == SceneManager.GetActiveScene().buildIndex + 1)
        {
            string content = "Finish This Level To Open Next Level!";
            NotificationParam param = new(content, true, () =>
            {
                _canClick = false;
                StartCoroutine(CooldownButton());
            });
            ShowNotificationHelper.ShowNotification(param);
        }
        else if (hp <= HP_ALARM_THRESHOLD)
        {
            if (hp == 0 && silverCoin <= DEFAULT_MINIMUM_SILVER_PLAYABLE)
            {
                string content = "Based On The Amount Of Silver You Have, You Are Granted 1 HP And 2 Temporary HP.";
                NotificationParam param = new NotificationParam(content, true, () =>
                {
                    _canClose = true;
                    OnClose();
                    if (levelToSwitch == _currentLevel)
                        UIManager.Instance.AnimateAndTransitionScene(levelToSwitch, true, true, true);
                    else
                        UIManager.Instance.AnimateAndTransitionScene(levelToSwitch, true, false, true);
                });
                ShowNotificationHelper.ShowNotification(param);
            }
            else if (hp == 0)
            {
                string content = "You Don't Have Any HealthPoint Left, Go Buy It In The Shop!";
                NotificationParam param = new(content, true, () =>
                {
                    StartCoroutine(DelayShopHelper.DelayOpenShop());
                });
                ShowNotificationHelper.ShowNotification(param);
            }
            else if (_currentSCoin < DEFAULT_MINIMUM_SILVER_PLAYABLE)
            {
                string content = "Based On The Amount Of Silver You Have, You Are Granted 2 Temporary HP.";
                NotificationParam param = new NotificationParam(content, true, () =>
                {
                    _canClose = true;
                    OnClose();
                    if (levelToSwitch == _currentLevel)
                        UIManager.Instance.AnimateAndTransitionScene(levelToSwitch, true, true, true);
                    else
                        UIManager.Instance.AnimateAndTransitionScene(levelToSwitch, true, false, true);
                });
                ShowNotificationHelper.ShowNotification(param);
            }
            else
            {
                string content = "You Only Have One HealthPoint Left, Buy It In The Shop Now?";
                NotificationParam param = new(content, false, null, () =>
                {
                    StartCoroutine(DelayShopHelper.DelayOpenShop());
                }, () =>
                {
                    _canClose = true;
                    OnClose();
                    if (levelToSwitch == _currentLevel)
                        UIManager.Instance.AnimateAndTransitionScene(levelToSwitch, true, true, false);
                    else
                        UIManager.Instance.AnimateAndTransitionScene(levelToSwitch, true, false, false);
                });
                ShowNotificationHelper.ShowNotification(param);
            }
        }
        else
        {
            _canClose = true;
            OnClose();
            if (levelToSwitch == _currentLevel)
                UIManager.Instance.AnimateAndTransitionScene(levelToSwitch, true, true);
            else
                UIManager.Instance.AnimateAndTransitionScene(levelToSwitch, true, true);
        }
    }
}
