using UnityEngine;

public class BunnyAttackJumpState : MEnemiesAttackState
{
    private BunnyManager _bunnyManager;
    private float _xAxisDistance;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _bunnyManager = (BunnyManager)charactersManager;
        Attack();
    }

    public override void ExitState() { }

    public override void Update() 
    {
        if (CheckIfCanFall())
            _bunnyManager.ChangeState(_bunnyManager.BunnyAttackFallState);
    }

    private bool CheckIfCanFall()
    {
        return _bunnyManager.GetRigidbody2D().velocity.y < 0.1f;
    }

    public override void FixedUpdate() { }

    protected override void Attack()
    {
        //Debug.Log("Bx, Px: " + _bunnyManager.transform.position.x + " " + _bunnyManager.PlayerRef.transform.position.x);
        _xAxisDistance = _bunnyManager.transform.position.x - _bunnyManager.PlayerRef.transform.position.x;
        //Debug.Log("Jump, rabb x: " + _xAxisDistance + ", " + _bunnyManager.transform.position.x);

        if (_xAxisDistance < 0)
            _bunnyManager.GetRigidbody2D().AddForce(new Vector2(Mathf.Abs(_xAxisDistance), _bunnyManager.JumpHeight), ForceMode2D.Impulse);
        else
            _bunnyManager.GetRigidbody2D().AddForce(new Vector2(-_xAxisDistance, _bunnyManager.JumpHeight), ForceMode2D.Impulse);
        //Impulse: A large force applied for a very short duration
    }
}
