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
        _mEnemiesManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EMEnemiesState.idle);
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
            //vì có thể đã Invoke attack player nhưng player ăn buff vô hình
            //=> cần cancel th attack
            _mEnemiesManager.CancelInvoke();
            _mEnemiesManager.ChangeState(_mEnemiesManager.MEnemiesPatrolState);
        }
        else if (CheckIfCanAttack())
        {
            _hasChangedState = true;
            _mEnemiesManager.Invoke("AllowAttackPlayer", _mEnemiesManager.EnemiesSO.AttackDelay);
        }
        //Debug.Log("Update");
    }

    protected virtual bool CheckIfCanPatrol()
    {
        return Time.time - _entryTime >= _mEnemiesManager.MEnemiesSO.RestTime;
    }

    protected virtual bool CheckIfCanAttack()
    {
        return _mEnemiesManager.HasDetectedPlayer && !_hasChangedState;
    }

    public override void FixedUpdate() { }
}
