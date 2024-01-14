using UnityEngine;

public class PigAttackRedState : MEnemiesAttackState
{
    private PigManager _pigManager;
    private float _xAxisDistance;

    public override void EnterState(CharactersManager charactersManager)
    {
        _pigManager = (PigManager)charactersManager;
        _pigManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EPigState.attackRed);
        _pigManager.HasGotHit = false;
        Debug.Log("Atk Red");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void Update()
    {
        _xAxisDistance = _pigManager.transform.position.x - _pigManager.PlayerRef.position.x;
        if (_xAxisDistance < 0 && !_pigManager.GetIsFacingRight() && Mathf.Abs(_xAxisDistance) >= GameConstants.PIG_FLIPABLE_RANGE)
            _pigManager.FlipRight();
        else if (_xAxisDistance > 0 && _pigManager.GetIsFacingRight() && Mathf.Abs(_xAxisDistance) >= GameConstants.PIG_FLIPABLE_RANGE)
            _pigManager.FlipLeft();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Attack()
    {
        if (_pigManager.GetIsFacingRight())
            _pigManager.GetRigidbody2D().velocity = new Vector2(_pigManager.ChaseSpeedRedForm, _pigManager.GetRigidbody2D().velocity.y);
        else
            _pigManager.GetRigidbody2D().velocity = new Vector2(-_pigManager.ChaseSpeedRedForm, _pigManager.GetRigidbody2D().velocity.y);
    }
}

