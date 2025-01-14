using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static GameEnums;
using static GameConstants;
using UnityEngine.UI;
using System;

public class ButtonCooldownController : ButtonSkillController
{
    [SerializeField] protected Image _skillImage;
    [SerializeField] protected Image _subImage;
    [SerializeField] protected TextMeshProUGUI _txtCooldown;
    protected float _timeLeft;
    protected bool _isCooldown;

    const float TWEEN_END_VALUE = 1.0f;
    const float TWEEN_RESET_VALUE = 0.0f;

    protected override void Awake()
    {
        base.Awake();
        gameObject.SetActive(_isActivated);
        _txtCooldown.gameObject.SetActive(false);
        EventsManager.SubcribeToAnEvent(EEvents.OnCooldownSkill, CooldownButton);
    }

    protected void OnDestroy()
    {
        EventsManager.UnsubscribeToAnEvent(EEvents.OnCooldownSkill, CooldownButton);
    }

    protected void CooldownButton(object obj = null)
    {
        SkillData data = (SkillData)obj;

        if (data.Name != _btnSkill) return;

        HandleCooldown(data.CooldownTime);
    }

    protected void HandleCooldown(float cooldownTime)
    {
        _subImage.DOFillAmount(TWEEN_END_VALUE, cooldownTime)
            .SetEase(Ease.Linear).OnComplete(() => _subImage.fillAmount = TWEEN_RESET_VALUE);

        _skillImage.color = new(0.45f, 0.45f, 0.45f, 1f);
        _txtCooldown.gameObject.SetActive(true);
        _timeLeft = cooldownTime;
        DOTween.To(() => _timeLeft, x => _timeLeft = x, 0, cooldownTime).OnUpdate(() =>
        {
            DisplayText();
        }).SetEase(Ease.Linear).OnComplete(() =>
        {
            _skillImage.color = new(1f, 1f, 1f, 1f);
            _txtCooldown.gameObject.SetActive(false);
            _isCooldown = false;
        });
    }

    protected virtual void DisplayText(string decimalType = DECIMAL_DISPLAY_ONE_PLACE)
    {
        TimeDisplayHelper.DisplayCooldownTime(ref _txtCooldown, _timeLeft, decimalType);
    }
}
