using UnityEngine;

public class MEnemiesIdleState : MEnemiesBaseState
{
    protected float _entryTime;
    protected bool _allowAttack;

    public void AllowAttack() { _allowAttack = true; }

    public override void EnterState(CharactersManager charactersManager)
    {
        //Gọi hàm dưới thì tham số charactersManager sẽ đc convert
        //sang biến kiểu EnemiesManager là _enemiesManager;
        base.EnterState(charactersManager);
        _mEnemiesManager.Animator.SetInteger("state", (int)EnumState.EMEnemiesState.idle);
        _mEnemiesManager.GetRigidbody2D().velocity = Vector2.zero;
        _entryTime = Time.time;
        Debug.Log("Idle");
    }

    public override void ExitState() 
    {
        _allowAttack = false;
    }

    public override void Update()
    {
        if (Time.time - _entryTime >= _mEnemiesManager.GetRestTime())
            _mEnemiesManager.ChangeState(_mEnemiesManager._mEnemiesPatrolState);
        else if (_mEnemiesManager.HasDetectedPlayer && _allowAttack)
            _mEnemiesManager.ChangeState(_mEnemiesManager._mEnemiesAttackState);
        //Debug.Log("Update");
    } 

    public override void FixedUpdate() { }
}
