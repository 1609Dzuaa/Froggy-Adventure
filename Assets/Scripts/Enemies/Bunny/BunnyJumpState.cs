using UnityEngine;

public class BunnyJumpState : BunnyBaseState
{
    private bool hasJump = false;
    public override void EnterState(BunnyStateManager bunnyStateManager)
    {
        base.EnterState(bunnyStateManager);
        _bunnyStateManager.GetAnimator().SetInteger("state", (int)EnumState.EBunnyState.jump);
        if (_bunnyStateManager.GetIsFacingRight())
            _bunnyStateManager.GetRigidBody2D().AddForce(_bunnyStateManager.GetJumpForce());
        else
            _bunnyStateManager.GetRigidBody2D().AddForce(new Vector2(-1 * _bunnyStateManager.GetJumpForce().x, _bunnyStateManager.GetJumpForce().y));
    }

    public override void ExitState() { hasJump = false; }

    public override void Update() 
    {
        if (Mathf.Abs(_bunnyStateManager.GetRigidBody2D().velocity.y) < 0.01f)
            _bunnyStateManager.ChangeState(_bunnyStateManager.bunnyFallState);
    }

    public override void FixedUpdate() 
    {
        //Có nên cho vào đây ?
        /*if (!hasJump)
        {
            hasJump = true;
            if (_bunnyStateManager.GetIsFacingRight())
                _bunnyStateManager.GetRigidBody2D().AddForce(_bunnyStateManager.GetJumpForce());
            else
                _bunnyStateManager.GetRigidBody2D().AddForce(new Vector2(-1 * _bunnyStateManager.GetJumpForce().x, _bunnyStateManager.GetJumpForce().y));
        }*/
    }

}
