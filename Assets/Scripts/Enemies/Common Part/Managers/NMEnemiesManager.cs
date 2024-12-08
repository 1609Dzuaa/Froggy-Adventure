using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public class NMEnemiesManager : EnemiesManager
{
    protected NMEnemiesIdleState _nmEnemiesIdleState = new();
    protected NMEnemiesAttackState _nmEnemiesAttackState = new();
    protected NMEnemiesGotHitState _nmEnemiesGotHitState = new();

    public NMEnemiesIdleState GetNMEnemiesIdleState { get { return _nmEnemiesIdleState; } set { _nmEnemiesIdleState = value; } }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void SetUpProperties()
    {
        base.SetUpProperties();
        _state = _nmEnemiesIdleState;
        _state.EnterState(this);
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void HandleIfBossDie(object obj)
    {
        _hasGotHit = true;
        _notPlayDeadSfx = true;
        ChangeState(_nmEnemiesGotHitState);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.collider.CompareTag(GameConstants.BULLET_TAG))
        {
            string bulletID = collision.collider.GetComponent<BulletController>().BulletID;
            EventsManager.Instance.NotifyObservers(EEvents.BulletOnHit, bulletID);
            ChangeState(_nmEnemiesGotHitState);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConstants.PLAYER_TAG) && !_hasGotHit && _state is not NMEnemiesGotHitState)
        {
            //Thêm ĐK: _state is not NMEnemiesGotHitState vì:
            //có thể enemy này dính đòn từ bullet => rotate z khi GotHit
            //và Trigger collider của Player dẫn đến JumpPassive
            _hasGotHit = true;
            EventsManager.Instance.NotifyObservers(EEvents.PlayerOnJumpPassive);
            SpawnRewardForPlayer();
            SpawnDeathFX(EPoolable.EnemyDeathSkullVfx);
            ChangeState(_nmEnemiesGotHitState);
            SpawnBountyIfMarked();
        }
        else if (collision.CompareTag(GameConstants.DEAD_ZONE_TAG) && !_hasGotHit && _state is not NMEnemiesGotHitState
            || collision.CompareTag(GameConstants.TRAP_TAG) && !_hasGotHit && _state is not NMEnemiesGotHitState)
        {
            _hasGotHit = true;
            ChangeState(_nmEnemiesGotHitState);
        }
    }

    protected virtual void AllowAttackPlayer()
    {
        if (BuffsManager.Instance.GetBuff(EBuffs.Invisible).IsActivating)
        {
            ChangeState(_nmEnemiesIdleState);
            return;
        }

        ChangeState(_nmEnemiesAttackState);
    }
}
