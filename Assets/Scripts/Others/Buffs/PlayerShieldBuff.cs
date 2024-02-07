using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShieldBuff : PlayerBuffs
{
    [SerializeField] private float _runningOutDuration;

    private Animator _anim;
    private CircleCollider2D _circleCollider2D;
    private bool _hasTriggeredRunningOut;
    private bool _hasDisabled;
    Transform _shieldPos;

    public override void Awake()
    {
        GetReferenceComponentsAndSetup();
    }

    private void GetReferenceComponentsAndSetup()
    {
        _anim = GetComponent<Animator>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _shieldPos = GameObject.Find("ShieldPosition").transform;
        _circleCollider2D.enabled = false;
    }

    public override void Update()
    {
        if (_isAllowToUpdate)
        {
            transform.position = _shieldPos.position;

            if (CheckIfRunningOut())
                HandleRunningOutState();
            else if (CheckIfCanDisable())
                DisableShield();
        }
    }

    public override void ApplyBuff()
    {
        //Reset lại data khi apply buff
        if (_shieldPos)
            transform.position = _shieldPos.position;
        _entryTime = Time.time;
        _isAllowToUpdate = true;
        _anim.SetTrigger("Idle");
        _hasTriggeredRunningOut = false;
        _hasDisabled = false;
        _circleCollider2D.enabled = true;
        SoundsManager.Instance.PlaySfx(GameEnums.ESoundName.ShieldBuffSfx, 1.0f);
    }

    private bool CheckIfRunningOut()
    {
        return Time.time - _entryTime >= _buffDuration && !_hasTriggeredRunningOut;
    }

    private void HandleRunningOutState()
    {
        _hasTriggeredRunningOut = true;
        _anim.SetTrigger(GameConstants.RUNNINGOUT);
        _entryTime = Time.time;
    }

    private bool CheckIfCanDisable()
    {
        return Time.time - _entryTime >= _runningOutDuration && _hasTriggeredRunningOut && !_hasDisabled;
    }

    private void DisableShield()
    {
        _hasDisabled = true;
        _isAllowToUpdate = false;
        _anim.SetTrigger("Disable");
        _circleCollider2D.enabled = false;
        SoundsManager.Instance.PlaySfx(GameEnums.ESoundName.SpecialBuffDebuffSfx, 1.0f);
    }

}