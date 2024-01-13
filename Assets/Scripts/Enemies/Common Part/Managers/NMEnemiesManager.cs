using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        _state = _nmEnemiesIdleState;
        _state.EnterState(this);
    }

    protected override void Update()
    {
        base.Update();
        //Debug.Log("update");
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.collider.CompareTag(GameConstants.BULLET_TAG))
        {
            EventsManager.Instance.NotifyObservers(GameEnums.EEvents.BulletOnHit, GameConstants.BULLET_ID);
            ChangeState(_nmEnemiesGotHitState);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConstants.PLAYER_TAG) && !_hasGotHit)
        {
            _hasGotHit = true;
            EventsManager.Instance.NotifyObservers(GameEnums.EEvents.PlayerOnJumpPassive, null);
            ChangeState(_nmEnemiesGotHitState);
        }
    }

    protected virtual void AllowAttackPlayer()
    {
        if (BuffsManager.Instance.GetTypeOfBuff(GameEnums.EBuffs.Invisible).IsAllowToUpdate)
            return;

        ChangeState(_nmEnemiesAttackState);
    }

    protected void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
