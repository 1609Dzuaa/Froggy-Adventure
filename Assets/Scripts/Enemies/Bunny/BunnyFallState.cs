using UnityEngine;

public class BunnyFallState : BunnyBaseState
{
    public override void EnterState(BunnyStateManager bunnyStateManager)
    {
        base.EnterState(bunnyStateManager);
        _bunnyStateManager.GetAnimator().SetInteger("state", (int)EnumState.EBunnyState.fall);

    }

    public override void ExitState() { }

    public override void Update() 
    {
        //Prob here
        if (Mathf.Abs(_bunnyStateManager.GetRigidBody2D().velocity.x) < 0.01f && Mathf.Abs(_bunnyStateManager.GetRigidBody2D().velocity.y) < 0.01f)
            _bunnyStateManager.ChangeState(_bunnyStateManager.bunnyIdleState);
    }

    public override void FixedUpdate() { }

}
