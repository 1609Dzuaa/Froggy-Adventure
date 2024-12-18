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

    private void Awake()
    {
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnUnlockSkill, DisplayAbility);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        TweenAbility(1f);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        TweenAbility(0f);
    }

    private void OnDestroy()
    {
        EventsManager.Instance.UnsubscribeToAnEvent(EEvents.OnUnlockSkill, DisplayAbility);
    }

    private void DisplayAbility(object obj)
    {
        SpecialItemStaticData sItemSData = obj as SpecialItemStaticData;
        _txtAbilityName.text = sItemSData.Ability.AbilityName.ToString();
        _txtAbilityDescribe.text = sItemSData.Ability.AbilityDescribe;
        _abilityImage.sprite = sItemSData.Ability.AbilityImage;
    }

    private void TweenAbility(float endValue)
    {
        _txtAbilityName.DOFade(endValue, _tweenDuration).SetEase(_tweenEase).OnComplete(() =>
        {
            _abilityImage.DOFade(endValue, _tweenDuration).SetEase(_tweenEase).OnComplete(() =>
            {
                _txtAbilityDescribe.DOFade(endValue, _tweenDuration).SetEase(_tweenEase);
            });
        });
    }
}
