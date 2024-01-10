using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NMEnemiesManager : EnemiesManager
{
    protected NMEnemiesIdleState _nmEnemiesIdleState = new();
    protected NMEnemiesAttackState _nmEnemiesAttackState = new();
    protected NMEnemiesGotHitState _nmEnemiesGotHitState = new();

    public NMEnemiesIdleState getNMEnemiesIdleState { get { return _nmEnemiesIdleState; } set { _nmEnemiesIdleState = value; } }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
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
        if (collision.collider.CompareTag(GameConstants.PLAYER_TAG))
        {
            var playerScript = collision.collider.GetComponent<PlayerStateManager>();
            playerScript.gotHitState.IsHitByTrap = true;
            EventsManager.Instance.InvokeAnEvent(GameEnums.EEvents.EnemiesOnDamagePlayer, null);
        }
        else if (collision.collider.CompareTag(GameConstants.BULLET_TAG))
        {
            EventsManager.Instance.InvokeAnEvent(GameEnums.EEvents.EnemiesOnDie, GameConstants.BULLET_ID);
            ChangeState(_nmEnemiesGotHitState);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConstants.PLAYER_TAG) && !_hasGotHit)
        {
            _hasGotHit = true;
            EventsManager.Instance.InvokeAnEvent(GameEnums.EEvents.EnemiesOnDie, GameConstants.PLAYER_ID);
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
        Destroy(this.gameObject);
    }
}
