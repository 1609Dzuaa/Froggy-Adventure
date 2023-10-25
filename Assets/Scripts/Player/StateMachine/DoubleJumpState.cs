using UnityEngine;

public class DoubleJumpState : BaseState
{
    //Chỉ có thể double jump khi đã và đang jump || fall
    public override void EnterState(BaseStateManager _baseStateManager)
    {
        if (_baseStateManager is PlayerStateManager)
        {
            playerStateManager.GetAnimator().SetInteger("state", (int)EnumState.EState.doubleJump);
        }

        playerStateManager.SetHasDbJump(true);
        playerStateManager.GetRigidBody2D().velocity = new Vector2(playerStateManager.GetRigidBody2D().velocity.x, playerStateManager.GetvY() * 0.9f);
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        UpdateHorizontalLogic();

        if (playerStateManager.GetRigidBody2D().velocity.y < -0.1f)
            playerStateManager.ChangeState(playerStateManager.fallState);
    }

    private void UpdateHorizontalLogic()
    {
        if (playerStateManager.GetDirX() != 0)
        {
            playerStateManager.FlippingSprite();

            playerStateManager.GetRigidBody2D().velocity = new Vector2(playerStateManager.GetvX() * playerStateManager.GetDirX(), playerStateManager.GetRigidBody2D().velocity.y);
        }
    }

    public override void FixedUpdate()
    {

    }
}
