using Unity.VisualScripting;
using UnityEngine;

public class JumpState : BaseState
{
    //Considering Coyote Time
    public override void EnterState(BaseStateManager stateManager)
    {
        if (stateManager is PlayerStateManager)
        {
            playerStateManager.GetAnimator().SetInteger("state", (int)EnumState.EState.jump);

            if (playerStateManager.GetPrevStateIsWallSlide())
                playerStateManager.FlipSpriteAfterWallSlide();
            //Debug.Log("Jump");
        }
        HandleJump();
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        UpdateJumpLogic();

        UpdateHorizontalLogic();
    }

    private void UpdateJumpLogic()
    {
        //Press S While Jump => Doube Jump
        if (Input.GetKeyDown(KeyCode.S))
            playerStateManager.ChangeState(playerStateManager.doubleJumpState);
        if (playerStateManager.GetRigidBody2D().velocity.y < -0.1f)
            playerStateManager.ChangeState(playerStateManager.fallState);
        //Prob here
        if (playerStateManager.GetIsWallTouch() && !playerStateManager.GetIsOnGround())
            playerStateManager.ChangeState(playerStateManager.wallSlideState);
    }

    private void HandleJump()
    {
        playerStateManager.GetRigidBody2D().velocity = new Vector2(playerStateManager.GetRigidBody2D().velocity.x, playerStateManager.GetvY());
        playerStateManager.GetJumpSound().Play();
    }

    private void UpdateHorizontalLogic()
    {
        if (playerStateManager.GetDirX() != 0)
        {
            playerStateManager.GetRigidBody2D().velocity = new Vector2(playerStateManager.GetvX() * playerStateManager.GetDirX(), playerStateManager.GetRigidBody2D().velocity.y);
        }
    }

    public override void FixedUpdate()
    {

    }

}
