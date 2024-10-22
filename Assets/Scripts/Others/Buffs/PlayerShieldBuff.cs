using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static GameEnums;
using static GameConstants;

public class PlayerShieldBuff : PlayerActiveBuffs
{
    [Header("Kiếm ShieldPos của Player kéo vào đây")]
    [SerializeField] Transform _shieldPos;

    private Animator _anim;
    private CircleCollider2D _circleCollider2D;

    public override void Awake()
    {
        base.Awake();
        GetReferenceComponentsAndSetup();
    }

    protected override void HandleActiveBuff()
    {
        ActiveShield();
        DOTween.To(() => _entryTime, x => _entryTime = x, TRASH_VALUE, BUFF_DURATION)
            .OnUpdate(() => transform.position = _shieldPos.position)
            .OnComplete(() =>
            {
                _anim.SetTrigger(RUNNINGOUT);
                _entryTime = 0;
                DOTween.To(() => _entryTime, x => _entryTime = x, TRASH_VALUE, BUFF_RUN_OUT_DURATION)
                .OnUpdate(() => transform.position = _shieldPos.position)
                .OnComplete(() =>
                {
                    DisableShield();
                    _isActivating = false;
                });
            });
        //Debug.Log("clicked");
    }

    private void ActiveShield()
    {
        gameObject.SetActive(true);
        if (_shieldPos)
            transform.position = _shieldPos.position;
        _anim.SetTrigger(IDLE);
        _circleCollider2D.enabled = true;
        _isActivating = true;
        SoundsManager.Instance.PlaySfx(ESoundName.ShieldBuffSfx, 1.0f);
    }

    private void GetReferenceComponentsAndSetup()
    {
        _anim = GetComponent<Animator>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _circleCollider2D.enabled = false;
    }

    /*public override void Update()
    {
        if (_isAllowToUpdate)
        {
            transform.position = _shieldPos.position;

            if (CheckIfRunningOut())
                HandleRunningOutState();
            else if (CheckIfCanDisable())
                DisableShield();
        }
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(BOSS_SHIELD_TAG) || collision.CompareTag(TRAP_TAG))
        {
            DisableShield();
            SpawnDeShieldVfx(collision.ClosestPoint(transform.position));
        }
        //Va phải boss thì thu hồi shield
    }

    private void SpawnDeShieldVfx(Vector2 position)
    {
        GameObject deShieldVfx = Pool.Instance.GetObjectInPool(EPoolable.PlayerDeShieldVfx);
        deShieldVfx.SetActive(true);
        deShieldVfx.transform.position = position;
    }

    private void DisableShield()
    {
        _anim.SetTrigger(DISABLE);
        _circleCollider2D.enabled = false;
        SoundsManager.Instance.PlaySfx(ESoundName.SpecialBuffDebuffSfx, 1.0f);
        gameObject.SetActive(false);
    }

}