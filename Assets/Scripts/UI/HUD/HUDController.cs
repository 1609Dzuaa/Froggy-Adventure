using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static GameEnums;
using DG.Tweening;
using Unity.Profiling;
using static GameConstants;

public class HUDController : MonoBehaviour
{
    //hp, time, coins, icons
    //chuyển đống này thành InGameUICanvas

    [SerializeField] TextMeshProUGUI _txtTimer;
    [SerializeField, Tooltip("Delay bộ đếm 1 khoảng nhỏ" +
        " lấy tương đối gần = thgian quá trình chuyển scene")] int _delayCount;
    [SerializeField] TweenCoin _tweenCoin;
    int _timeLeft, _timeAllow = 0, _bonusTime = 0;
    Tween _timerTween;
    private static ProfilerMarker performanceMarker = new ProfilerMarker("ImprovedCode");

    // Start is called before the first frame update
    void Start()
    {
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnSetupTimeAllow, SetupAndCountTime);
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnReturnMainMenu, KillTweenTimer);
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnUnlockSkill, BonusTimer);
    }

    private void OnDestroy()
    {
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.OnSetupTimeAllow, SetupAndCountTime);
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.OnReturnMainMenu, KillTweenTimer);
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.OnUnlockSkill, BonusTimer);
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

    private void SetupAndCountTime(object obj)
    {
        performanceMarker.Begin();
        _timeLeft = _timeAllow = (int)obj + _bonusTime;
        _timeLeft += _delayCount;
        TimeDisplayHelper.DisplayTime(ref _txtTimer, _timeLeft, _timeAllow);
        _timerTween = DOTween.To(() => _timeLeft, x => _timeLeft = x, 0, _timeLeft).OnUpdate(() =>
        {
            TimeDisplayHelper.DisplayTime(ref _txtTimer, _timeLeft, _timeAllow);
        }).SetEase(Ease.Linear).OnComplete(() =>
        {
            //truyen them tham so fail hay win
            int timeComplete = _timeAllow - _timeLeft;
            ResultParam pr = new(ELevelResult.Failed, _tweenCoin.SCoinCollected, _tweenCoin.GCoinCollected, timeComplete, _timeAllow);
            UIManager.Instance.TogglePopup(EPopup.Result, true);
            EventsManager.Instance.NotifyObservers(EEvents.OnFinishLevel, pr);
        });
        performanceMarker.End();
    }

    private void KillTweenTimer(object obj)
    {
        _timerTween.Kill();
    }

    private void BonusTimer(object obj)
    {
        SpecialItemStaticData sItemSData = obj as SpecialItemStaticData;
        if (sItemSData.Ability.AbilityName == ESkills.Hourglass)
            _bonusTime = HOURGLASS_BONUS_TIME;
    }
}
