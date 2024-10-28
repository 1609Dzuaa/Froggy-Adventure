using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static GameEnums;
using DG.Tweening;
using Unity.Profiling;
using static GameConstants;
using UnityEngine.SceneManagement;

[System.Serializable]
public class BuffIcon
{
    public ESkills Name;
    public GameObject Icon;
}

public class HUDController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _txtTimer;
    [SerializeField, Tooltip("Delay bộ đếm 1 khoảng nhỏ" +
        " lấy tương đối gần = thgian quá trình chuyển scene")] int _delayCount;
    [SerializeField] TweenCoin _tweenCoin;
    [SerializeField] BuffIcon[] _arrBuffIcons;
    int _timeLeft, _timeAllow = 0, _bonusTime = 0;
    Tween _timerTween;
    LevelInfo _levelInfo;
    private static ProfilerMarker performanceMarker = new ProfilerMarker("ImprovedCode");

    void Awake()
    {
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnSetupLevel, SetupHUD);
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnReturnMainMenu, KillTweenTimer);
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnResetLevel, HandleReset);
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnLevelCompleted, ReceiveLevelResult);
    }

    private void OnDestroy()
    {
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.OnSetupLevel, SetupHUD);
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.OnReturnMainMenu, KillTweenTimer);
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.OnResetLevel, HandleReset);
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.OnLevelCompleted, ReceiveLevelResult);
    }

    private void HandleDisplayBuffIcons()
    {
        var listLimitedSkills = ToggleAbilityItemHelper.GetListActivatedSkills();
        foreach (var item in _arrBuffIcons)
            item.Icon.SetActive(listLimitedSkills.Find(x => x.SkillName == item.Name) != null);
    }

    /*private void Update()
    {
        performanceMarker.Begin();
        if (Time.time - _timer >= 1f)
        {
            _count--;
            _timer = Time.time;
            TimeDisplayHelper.DisplayTime(ref _txtTimer, _timeLeft, 10);
        }
        performanceMarker.End();
    }*/

    private void SetupHUD(object obj)
    {
        //performanceMarker.Begin();
        LevelInfo info = (LevelInfo)obj;
        _levelInfo = info;
        _bonusTime = info.ListActiveSkills.Find(x => x.SkillName == ESkills.Hourglass) != null ? HOURGLASS_BONUS_TIME : 0;
        _timeLeft = _timeAllow = info.LevelTimeAllow + _bonusTime;
        TimeDisplayHelper.DisplayTime(ref _txtTimer, _timeLeft, _timeAllow);
        HandleDisplayBuffIcons();
        //performanceMarker.End();
    }

    public void Countdown()
    {
        _timerTween = DOTween.To(() => _timeLeft, x => _timeLeft = x, 0, _timeLeft).OnUpdate(() =>
        {
            TimeDisplayHelper.DisplayTime(ref _txtTimer, _timeLeft, _timeAllow);
        }).SetEase(Ease.Linear).OnComplete(() =>
        {
            HandleFinishLevel();
        });
        //Debug.Log("start Count");
    }

    private void ReceiveLevelResult(object obj)
    {
        HandleFinishLevel((ELevelResult)obj);
    }

    private void HandleFinishLevel(ELevelResult result = ELevelResult.Failed)
    {
        //truyen them tham so fail hay win
        int timeComplete = _timeAllow - _timeLeft;
        ResultParam pr = new(result, _tweenCoin.SCoinCollected, _tweenCoin.GCoinCollected, timeComplete, _timeAllow);
        UIManager.Instance.TogglePopup(EPopup.Result, true);
        SaveLevelProgress();
        KillTweenTimer();
        EventsManager.Instance.NotifyObservers(EEvents.OnHandleLevelCompleted, pr);
        EventsManager.Instance.NotifyObservers(EEvents.OnSavePlayerData);
    }

    private void KillTweenTimer(object obj = null)
    {
        _timerTween.Kill();
        //Debug.Log(_timerTween.IsActive());
        _bonusTime = 0;
        EventsManager.Instance.NotifyObservers(EEvents.OnLockLimitedSkills);
        //lock item trước r mới check sau
        //Debug.Log("Kill");
    }

    private void HandleReset(object obj)
    {
        _tweenCoin.ResetCoins();

        //TH replay level, có mua cgi
        //thì gọi cái list active skill
        //r tìm trong đó cái skill cần
        List<Skills> listActiveSkills = ToggleAbilityItemHelper.GetListActivatedSkills();
        _bonusTime = listActiveSkills.Find(x => x.SkillName == ESkills.Hourglass) != null ? HOURGLASS_BONUS_TIME : 0;
        _timeLeft = _timeAllow = _levelInfo.LevelTimeAllow + _bonusTime;
        TimeDisplayHelper.DisplayTime(ref _txtTimer, _timeAllow, _timeAllow);
        HandleDisplayBuffIcons();
        //Debug.Log("Reset");
    }

    private void SaveLevelProgress()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        LevelProgressData levelPData = new(currentLevelIndex, true, true, _timeAllow - _timeLeft);
        string levelFilePath = Application.persistentDataPath + LEVEL_DATA_PATH + currentLevelIndex.ToString() + ".json";
        JSONDataHelper.SaveToJSon<LevelProgressData>(levelPData, levelFilePath);

        int nextLevelIndex = currentLevelIndex + 1;
        LevelProgressData nextLevelPData = new(nextLevelIndex, true, false, 0);
        string nextLevelFilePath = Application.persistentDataPath + LEVEL_DATA_PATH + nextLevelIndex.ToString() + ".json";
        JSONDataHelper.SaveToJSon<LevelProgressData>(nextLevelPData, nextLevelFilePath);
        EventsManager.Instance.NotifyObservers(EEvents.OnUpdateLevel, nextLevelIndex);
    }
}
