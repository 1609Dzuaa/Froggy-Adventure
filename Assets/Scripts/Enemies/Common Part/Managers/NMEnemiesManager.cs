using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NMEnemiesManager : EnemiesManager
{
    private NMEnemiesIdleState _nmEnemiesIdleState = new();
    private NMEnemiesAttackState _nmEnemiesAttackState = new();
    private NMEnemiesGotHitState _nmEnemiesGotHitState = new();

    public NMEnemiesIdleState getNMEnemiesIdleState { get { return _nmEnemiesIdleState; } set { _nmEnemiesIdleState = value; } }

    public NMEnemiesAttackState getNMEnemiesAttackState { get { return _nmEnemiesAttackState; } }

    public NMEnemiesGotHitState getNMEnemiesGotHitState { get { return _nmEnemiesGotHitState; } }

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
        if (collision.collider.name == GameConstants.PLAYER_NAME)
        {
            var playerScript = collision.collider.GetComponent<PlayerStateManager>();
            playerScript.gotHitState.IsHitByTrap = true;
            playerScript.ChangeState(playerScript.gotHitState);
        }
        else if(collision.collider.CompareTag(GameConstants.BULLET_TAG))
        {
            var BulletCtrl = collision.collider.GetComponent<BulletController>();
            BulletCtrl.SpawnBulletPieces();
            Destroy(BulletCtrl.gameObject);
            ChangeState(_nmEnemiesGotHitState);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == GameConstants.PLAYER_NAME && !_hasGotHit)
        {
            _hasGotHit = true;
            var playerScript = collision.GetComponent<PlayerStateManager>();
            playerScript.SetCanDbJump(true); //Nhảy lên đầu Enemies thì cho phép DbJump tiếp
            playerScript.ChangeState(playerScript.jumpState);
            ChangeState(_nmEnemiesGotHitState);
        }
    }

    protected virtual void AllowAttackPlayer()
    {
        if (PlayerInvisibleBuff.Instance.IsAllowToUpdate)
            return;

        ChangeState(_nmEnemiesAttackState);
    }

    private void SelfDestroy()
    {
        Destroy(this.gameObject);
    }
}
