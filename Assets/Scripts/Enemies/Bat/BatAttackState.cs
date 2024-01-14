using UnityEngine;

public class BatAttackState : MEnemiesAttackState
{
    private float _xAxisDistance;
    private BatManager _batManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _batManager = (BatManager)charactersManager;
        _batManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EBatState.attack);
        _batManager.GetRigidbody2D().velocity = Vector2.zero;
        //Debug.Log("Attack");
    }

    public override void ExitState() { }

    public override void Update() 
    {
        if (CheckIfNeedRetreat())
            _batManager.ChangeState(_batManager.BatRetreatState);
        else if (CheckIfCanIdle())
            _batManager.ChangeState(_batManager.BatIdleState);
        else
            Attack();
    }

    private bool CheckIfCanIdle()
    {
        //Maybe prob here
        //Tương tự như bee, khi chase mà player tàng hình thì về idle
        return !_batManager.HasDetectedPlayer;
    }

    public override void FixedUpdate() { }

    protected override void Attack()
    {
        _xAxisDistance = _batManager.transform.position.x - _batManager.PlayerRef.position.x;
        if (_xAxisDistance < 0 && !_batManager.GetIsFacingRight() && Mathf.Abs(_xAxisDistance) >= GameConstants.BAT_FLIPABLE_RANGE)
            _batManager.FlipRight();
        else if (_xAxisDistance > 0 && _batManager.GetIsFacingRight() && Mathf.Abs(_xAxisDistance) >= GameConstants.BAT_FLIPABLE_RANGE)
            _batManager.FlipLeft();

        //Move vật thể theo target
        _batManager.transform.position = Vector2.MoveTowards(_batManager.transform.position, _batManager.PlayerRef.position, _batManager.MEnemiesSO.ChaseSpeed.x * Time.deltaTime);
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

        //Trong lúc Attack mà vượt Boundaries thì Retreat về tổ
    }

}
