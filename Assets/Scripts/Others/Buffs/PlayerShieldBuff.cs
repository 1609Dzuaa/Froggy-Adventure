using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static GameEnums;
using static GameConstants;

public class PlayerShieldBuff : PlayerBuffs
{
    [SerializeField] private Transform _deShieldVfxPos;

    private Animator _anim;
    private CircleCollider2D _circleCollider2D;
    Transform _shieldPos;

    public override void Awake()
    {
        GetReferenceComponentsAndSetup();
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnUseSkill, PerformSkill);
    }

    private void OnDestroy()
    {
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.OnUseSkill, PerformSkill);
    }

    private void PerformSkill(object obj)
    {
        ESkills name = (ESkills)obj;
        if (name == ESkills.Shield)
        {
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
                    });
                });
        }
    }

    private void GetReferenceComponentsAndSetup()
    {
        _anim = GetComponent<Animator>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _shieldPos = GameObject.Find("ShieldPosition").transform;
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

    public override void ApplyBuff()
    {
        gameObject.SetActive(true);
        //Reset lại data khi apply buff
        if (_shieldPos)
            transform.position = _shieldPos.position;
        //_entryTime = Time.time;
        _isAllowToUpdate = true;
        _anim.SetTrigger("Idle");
        _circleCollider2D.enabled = true;
        SoundsManager.Instance.PlaySfx(ESoundName.ShieldBuffSfx, 1.0f);
    }

    private void DisableShield()
    {
        //_isAllowToUpdate = false;
        _anim.SetTrigger("Disable");
        _circleCollider2D.enabled = false;
        SoundsManager.Instance.PlaySfx(ESoundName.SpecialBuffDebuffSfx, 1.0f);
        gameObject.SetActive(false);
    }

}