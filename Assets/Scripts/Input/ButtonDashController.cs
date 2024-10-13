using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static GameEnums;
using DG.Tweening;
using TMPro;

public class ButtonDashController : ButtonSkillController
{
    [SerializeField] Image _subImage;
    [SerializeField] TextMeshProUGUI _txtCooldown;
    private bool _isDashing = false;
    private float _timeLeft;
    const float TWEEN_END_VALUE = 1.0f;
    const float TWEEN_RESET_VALUE = 0.0f;

    public bool IsDashing { get => _isDashing; }

    protected override void Awake()
    {
        base.Awake();
        gameObject.SetActive(_isActivated);
        _txtCooldown.gameObject.SetActive(false);
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnCooldownSkill, CooldownDash);
    }

    private void OnDestroy()
    {
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.OnCooldownSkill, CooldownDash);
    }

    private void CooldownDash(object obj)
    {
        //float data.CooldownTime = (float)obj;
        SkillData data = (SkillData)obj;

        if (data.Name != ESkills.Dash) return;

        _subImage.DOFillAmount(TWEEN_END_VALUE, data.CooldownTime)
            .SetEase(Ease.Linear).OnComplete(() => _subImage.fillAmount = TWEEN_RESET_VALUE);

        _txtCooldown.gameObject.SetActive(true);
        _timeLeft = data.CooldownTime;
        DOTween.To(() => _timeLeft, x => _timeLeft = x, 0, data.CooldownTime).OnUpdate(() =>
        {
            TimeDisplayHelper.DisplayCooldownTime(ref _txtCooldown, _timeLeft);
        }).SetEase(Ease.Linear).OnComplete(() =>
        {
            _txtCooldown.gameObject.SetActive(false);
        });
    }

    public void Dash(InputAction.CallbackContext context)
    {
        _isDashing = context.performed;
    }
}
