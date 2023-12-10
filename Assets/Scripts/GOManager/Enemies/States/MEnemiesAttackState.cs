using UnityEngine;

public class MEnemiesAttackState : MEnemiesBaseState
{
    protected bool _allowUpdate;

    public void SetAllowUpdate(bool para) { this._allowUpdate = para; }

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _mEnemiesManager.Animator.SetInteger("state", (int)EnumState.EMEnemiesState.attack);
        //Debug.Log("Attack");
    }

    public override void ExitState() { }

    public override void Update()
    {
        LogicUpdate();
        Debug.Log("Base Update");
    }

    private void LogicUpdate()
    {
        if (!_mEnemiesManager.HasDetectedPlayer)
        {
            //Debug.Log("Here");
            _mEnemiesManager.ChangeState(_mEnemiesManager.MEnemiesIdleState);
        }
    }

    public override void FixedUpdate()
    {
        Attack();
        Debug.Log("Fixed");
    }

    protected virtual void Attack()
    {
        //Base Attack will be chasing Player
        //Any Attack State derived from this class can ovveride this func
        //to perform Different Attack
        if (_mEnemiesManager.GetIsFacingRight())
            _mEnemiesManager.GetRigidbody2D().velocity = new Vector2(_mEnemiesManager.GetChaseSpeed(), _mEnemiesManager.GetRigidbody2D().velocity.y);
        else
            _mEnemiesManager.GetRigidbody2D().velocity = new Vector2(-_mEnemiesManager.GetChaseSpeed(), _mEnemiesManager.GetRigidbody2D().velocity.y);
    }

}
