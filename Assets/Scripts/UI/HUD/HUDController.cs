﻿using System.Collections;
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
    [SerializeField] HUDCoin _HUDCoin;
    [SerializeField] BuffIcon[] _arrBuffIcons;
    int _timeLeft, _timeAllow = 0, _bonusTime = 0;
    Tween _timerTween;
    LevelInfo _levelInfo;
    private static ProfilerMarker performanceMarker = new ProfilerMarker("ImprovedCode");

    void Awake()
    {
        EventsManager.SubcribeToAnEvent(EEvents.OnSetupLevel, SetupHUD);
        EventsManager.SubcribeToAnEvent(EEvents.OnReturnMainMenu, KillTweenTimer);
        EventsManager.SubcribeToAnEvent(EEvents.OnResetLevel, HandleReset);
        EventsManager.SubcribeToAnEvent(EEvents.OnLevelCompleted, ReceiveLevelResult);
    }

    private void OnDestroy()
    {
        EventsManager.UnsubscribeToAnEvent(EEvents.OnSetupLevel, SetupHUD);
        EventsManager.UnsubscribeToAnEvent(EEvents.OnReturnMainMenu, KillTweenTimer);
        EventsManager.UnsubscribeToAnEvent(EEvents.OnResetLevel, HandleReset);
        EventsManager.UnsubscribeToAnEvent(EEvents.OnLevelCompleted, ReceiveLevelResult);
    }

    private void HandleDisplayBuffIcons()
    {
        var listLimitedSkills = ToggleAbilityItemHelper.GetListActivatedSkills();
        foreach (var item in _arrBuffIcons)
            item.Icon.SetActive(listLimitedSkills.Find(x => x.SkillName == item.Name) != null);
    }

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

    //Ngưng hoặc resume tween timer khi replay scene (replay lúc die)
    public void ControlTweenTimer(bool isStop)
    {
        if (isStop) _timerTween.Pause();
        else _timerTween.Play();
    }

    public void Countdown(float delay = 0f)
    {
        _timerTween = DOTween.To(() => _timeLeft, x => _timeLeft = x, 0, _timeLeft + delay).OnUpdate(() =>
        {
            TimeDisplayHelper.DisplayTime(ref _txtTimer, _timeLeft, _timeAllow);
        }).SetEase(Ease.Linear).OnComplete(() =>
        {
            HandleFinishLevel();
        });
        Debug.Log("start Count");
    }

    private void ReceiveLevelResult(object obj)
    {
        HandleFinishLevel((ELevelResult)obj);
    }

    private void HandleFinishLevel(ELevelResult result = ELevelResult.Failed)
    {
        //truyen them tham so fail hay win
        int timeComplete = _timeAllow - _timeLeft;
        ResultParam pr = new(result, _HUDCoin.SCoinCollected, _HUDCoin.GCoinCollected, timeComplete, _timeAllow);
        SaveLevelProgress(result);
        KillTweenTimer();
        EventsManager.NotifyObservers(EEvents.OnHandleLevelCompleted, pr);
        EventsManager.NotifyObservers(EEvents.OnSavePlayerData);
        UIManager.Instance.TogglePopup(EPopup.Result, true);
    }

    private void KillTweenTimer(object obj = null)
    {
        _timerTween.Kill();
        _timerTween = null;
        //Debug.Log(_timerTween.IsActive());
        _bonusTime = 0;
        EventsManager.NotifyObservers(EEvents.OnLockLimitedSkills);
        //lock item trước r mới check sau
        //Debug.Log("Kill");
    }

    private void HandleReset(object obj)
    {
        _HUDCoin.ResetCoins();

        //TH replay level, có mua cgi
        //thì gọi cái list active skill
        //r tìm trong đó cái skill cần
        List<Skills> listActiveSkills = ToggleAbilityItemHelper.GetListActivatedSkills();
        _bonusTime = listActiveSkills.Find(x => x.SkillName == ESkills.Hourglass) != null ? HOURGLASS_BONUS_TIME : 0;
        _timeLeft = _timeAllow = _levelInfo.LevelTimeAllow + _bonusTime;
        TimeDisplayHelper.DisplayTime(ref _txtTimer, _timeAllow, _timeAllow);
        HandleDisplayBuffIcons();

        bool isResetWithoutCD = (bool)obj;
        if (!isResetWithoutCD)
            Countdown(0.5f);
        //Debug.Log("Reset");
    }

    private void SaveLevelProgress(ELevelResult res)
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        bool passedCurrentLevel = res == ELevelResult.Completed;
        string currentLevelFilePath = Application.persistentDataPath + LEVEL_DATA_PATH + currentLevelIndex.ToString() + ".json";
        LevelProgressData currentLevelPData = JSONDataHelper.LoadFromJSon<LevelProgressData>(currentLevelFilePath);

        //nếu level hiện tại ch xong thì mới xét đk để set done time = 0
        //còn nếu xong r thì xét đk để set done time nào nhanh nhất
        if (!currentLevelPData.IsCompleted)
            currentLevelPData = new(currentLevelIndex, true, passedCurrentLevel, (passedCurrentLevel) ? _timeAllow - _timeLeft : 0);
        else
        {
            int prevDoneTime = currentLevelPData.TimeCompleted;
            if (prevDoneTime > _timeAllow - _timeLeft)
                currentLevelPData = new(currentLevelIndex, true, passedCurrentLevel, _timeAllow - _timeLeft);
        }

        JSONDataHelper.SaveToJSon<LevelProgressData>(currentLevelPData, currentLevelFilePath);

        //lvl hiện tại là max, kh xử lý nữa
        if (currentLevelIndex == MAX_GAME_LEVEL - 1) return;

        int nextLevelIndex = currentLevelIndex + 1;
        string nextLevelFilePath = Application.persistentDataPath + LEVEL_DATA_PATH + nextLevelIndex.ToString() + ".json";
        LevelProgressData nextLevelPData = JSONDataHelper.LoadFromJSon<LevelProgressData>(nextLevelFilePath);

        //chỉ xét unlock nếu lvl kế ch đc unlock
        if (!nextLevelPData.IsUnlock)
        {
            nextLevelPData = new(nextLevelIndex, passedCurrentLevel, false, 0);
            JSONDataHelper.SaveToJSon<LevelProgressData>(nextLevelPData, nextLevelFilePath);
        }

        //bắn để cập nhật thông tin level (open level, time completed, ...)
        EventsManager.NotifyObservers(EEvents.OnUpdateLevel);
    }
}
