using UnityEngine;

public class MEnemiesIdleState : MEnemiesBaseState
{
    protected float _entryTime;
    protected bool _hasChangedState;

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
        _hasChangedState = false;
    }

    public override void Update()
    {
        if (CheckIfCanPatrol())
            _mEnemiesManager.ChangeState(_mEnemiesManager.MEnemiesPatrolState);
        else if (CheckIfCanAttack())
            _mEnemiesManager.Invoke("AllowAttackPlayer", _mEnemiesManager.GetAttackDelay());
        //Debug.Log("Update");
    }

    protected bool CheckIfCanPatrol()
    {
        if (Time.time - _entryTime >= _mEnemiesManager.GetRestTime() && !_hasChangedState)
        {
            _hasChangedState = true;
            return true;
        }
        return false;
    }

    protected bool CheckIfCanAttack()
    {
        if (_mEnemiesManager.HasDetectedPlayer && !_hasChangedState)
        {
            _hasChangedState = true;
            return true;
        }
        return false;
    }

    public override void FixedUpdate() { }
}
