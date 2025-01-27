using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using static GameEnums;

public class PopupNewAbility : PopupController
{
    [SerializeField] TextMeshProUGUI _txtAbilityName;
    [SerializeField] TextMeshProUGUI _txtAbilityDescribe;
    [SerializeField] Image _abilityImage;

    [Header("Tween riêng của Popup này")]
    [SerializeField] float _tweenDuration;
    [SerializeField] Ease _tweenEase;

    const float DEFAULT_TWEEN_VALUE = 0.01f;

    private void Awake()
    {
        EventsManager.SubcribeToAnEvent(EEvents.OnUnlockSkill, DisplayAbility);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    public override void OnOpen()
    {
        base.OnOpen();
        TweenAbility(1f, false);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        TweenAbility(0f);
    }

    private void OnDestroy()
    {
        EventsManager.UnsubscribeToAnEvent(EEvents.OnUnlockSkill, DisplayAbility);
    }

    private void DisplayAbility(object obj)
    {
        SpecialItemStaticData sItemSData = obj as SpecialItemStaticData;
        _txtAbilityName.text = sItemSData.Ability.AbilityName.ToString();
        _txtAbilityDescribe.text = sItemSData.Ability.AbilityDescribe;
        _abilityImage.sprite = sItemSData.Ability.AbilityImage;
    }

    private void TweenAbility(float endValue, bool useDefaultVal = true)
    {
        _txtAbilityName.DOFade(endValue, (!useDefaultVal) ? _tweenDuration : DEFAULT_TWEEN_VALUE).SetEase(_tweenEase).OnComplete(() =>
        {
            _abilityImage.DOFade(endValue, (!useDefaultVal) ? _tweenDuration : DEFAULT_TWEEN_VALUE).SetEase(_tweenEase).OnComplete(() =>
            {
                _txtAbilityDescribe.DOFade(endValue, (!useDefaultVal) ? _tweenDuration : DEFAULT_TWEEN_VALUE).SetEase(_tweenEase);
            });
        });
    }
}
