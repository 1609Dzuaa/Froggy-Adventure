using UnityEngine;

public class MEnemiesIdleState : MEnemiesBaseState
{
    //Thằng hasChangedState tạo ra nhằm mục đích duy nhất là chỉ Invoke Attack player 1 lần
    //Muốn change sang state != attack player thì đ' cần check nó

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
        //Debug.Log("Idle, HitWall: " + _mEnemiesManager.HasCollidedWall);
    }

    public override void ExitState() 
    {
        _hasChangedState = false;
    }

    public override void Update()
    {
        if (CheckIfCanPatrol())
        {
            _mEnemiesManager.CancelInvoke();
            _mEnemiesManager.ChangeState(_mEnemiesManager.MEnemiesPatrolState);
        }
        else if (CheckIfCanAttack())
        {
            _hasChangedState = true;
            _mEnemiesManager.Invoke("AllowAttackPlayer", _mEnemiesManager.GetAttackDelay());
        }
        //Debug.Log("Update");
    }

    protected bool CheckIfCanPatrol()
    {
        return Time.time - _entryTime >= _mEnemiesManager.GetRestTime();
    }

    protected virtual bool CheckIfCanAttack()
    {
        return _mEnemiesManager.HasDetectedPlayer && !_hasChangedState;
    }

    public override void FixedUpdate() { }
}
