using UnityEngine;

public class PigAttackRedState : MEnemiesAttackState
{
    private PigManager _pigManager;
    private bool _allowFlip = true;

    public bool AllowFlip { set { _allowFlip = value; } }

    public override void EnterState(CharactersManager charactersManager)
    {
        _pigManager = (PigManager)charactersManager;
        _pigManager.Animator.SetInteger("state", (int)EnumState.EPigState.attackRed);
        _pigManager.HasGotHit = false;
        Debug.Log("Atk Red");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void Update()
    {
        //StartCor để delay việc bật lại cờ cho phép flip nhằm tránh flip loạn xạ
        if (_pigManager.transform.position.x >= _pigManager.PlayerRef.position.x && _pigManager.GetIsFacingRight() && _allowFlip)
        {
            _allowFlip = false;
            _pigManager.FlipLeft();
            _pigManager.StartCoroutine(_pigManager.AllowFlip());
        }
        else if (_pigManager.transform.position.x < _pigManager.PlayerRef.position.x && !_pigManager.GetIsFacingRight() && _allowFlip)
        {
            _allowFlip = false;
            _pigManager.FlipRight();
            _pigManager.StartCoroutine(_pigManager.AllowFlip());
        }
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

