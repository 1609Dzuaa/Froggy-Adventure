using UnityEngine;

public class BatAttackState : MEnemiesAttackState
{
    private float _xAxisDistance;
    private BatManager _batManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _batManager = (BatManager)charactersManager;
        _batManager.Animator.SetInteger("state", (int)EnumState.EBatState.attack);
        //Debug.Log("Attack");
    }

    public override void ExitState() { }

    public override void Update() 
    {
        if (CheckIfNeedRetreat())
            _batManager.ChangeState(_batManager.BatRetreatState);
        else
            Attack();
    }

    public override void FixedUpdate() { }

    protected override void Attack()
    {
        _xAxisDistance = _batManager.transform.position.x - _batManager.Player.position.x;
        if (_xAxisDistance < 0 && !_batManager.GetIsFacingRight())
            _batManager.FlipRight();
        else if (_xAxisDistance > 0 && _batManager.GetIsFacingRight())
            _batManager.FlipLeft();

        //Move vật thể theo target
        _batManager.transform.position = Vector2.MoveTowards(_batManager.transform.position, _batManager.Player.position, _batManager.GetChaseSpeed() * Time.deltaTime);
    }

    private bool CheckIfNeedRetreat()
    {
        if (_batManager.transform.position.x >= _batManager.BoundaryRight.position.x)
        {
            _batManager.FlipLeft();
            return true;
        }
        else if(_batManager.transform.position.x <= _batManager.BoundaryLeft.position.x)
        {
            _batManager.FlipRight();
            return true;
        }
        return false;
    }

}
