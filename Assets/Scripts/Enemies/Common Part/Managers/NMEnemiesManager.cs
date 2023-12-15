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

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player" && !_hasGotHit)
        {
            _hasGotHit = true;
            var playerScript = collision.GetComponent<PlayerStateManager>();
            playerScript.SetCanDbJump(true); //Nhảy lên đầu Enemies thì cho phép DbJump tiếp
            playerScript.GetRigidBody2D().AddForce(playerScript.GetJumpOnEnemiesForce(), ForceMode2D.Impulse);
            ChangeState(_nmEnemiesGotHitState);
        }
    }

    protected virtual void AllowAttackPlayer()
    {
        ChangeState(_nmEnemiesAttackState);
    }

    private void SelfDestroy()
    {
        Destroy(this.gameObject);
    }
}
