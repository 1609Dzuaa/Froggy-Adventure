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

        HandleDbJump();
        //Debug.Log("DBJump");
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        UpdateHorizontalLogic();
        UpdateVerticalLogic();
    }

    private void UpdateHorizontalLogic()
    {
        if (playerStateManager.GetDirX() != 0)
        {
            playerStateManager.GetRigidBody2D().velocity = new Vector2(playerStateManager.GetSpeedX() * playerStateManager.GetDirX(), playerStateManager.GetRigidBody2D().velocity.y);
        }
    }

    private void UpdateVerticalLogic()
    {
        if (playerStateManager.GetRigidBody2D().velocity.y < -0.1f)
            playerStateManager.ChangeState(playerStateManager.fallState);
        else if (playerStateManager.GetIsWallTouch() && !playerStateManager.GetIsOnGround())
            playerStateManager.ChangeState(playerStateManager.wallSlideState);
    }

    public override void FixedUpdate()
    {
        
    }

    private void HandleDbJump()
    {
        playerStateManager.SetHasDbJump(true);
        playerStateManager.GetRigidBody2D().velocity = new Vector2(playerStateManager.GetRigidBody2D().velocity.x, playerStateManager.GetSpeedY() * 0.9f);
    }
}
