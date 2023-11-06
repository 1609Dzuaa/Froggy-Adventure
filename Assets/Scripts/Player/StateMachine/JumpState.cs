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

            HandleJump();
            //Nếu state trước là WS thì tức là đang WallJump
            if (playerStateManager.GetPrevStateIsWallSlide())
            {
                playerStateManager.FlipSpriteAfterWallSlide();
            }
            Debug.Log("Jump");
        }
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
        //Press S While Jump => Double Jump
        if (Input.GetKeyDown(KeyCode.S))
            playerStateManager.ChangeState(playerStateManager.doubleJumpState);
        else if (playerStateManager.GetRigidBody2D().velocity.y < -0.1f)
        {
            //Debug.Log("Change here: " + playerStateManager.GetRigidBody2D().velocity.y);
            playerStateManager.ChangeState(playerStateManager.fallState);
        }
        else if (playerStateManager.GetIsWallTouch() && !playerStateManager.GetIsOnGround())
        {
            playerStateManager.ChangeState(playerStateManager.wallSlideState);
        }
    }

    private void HandleJump()
    {
        //Nếu nhảy khi đang trượt tường thì nhảy xéo
        if (!playerStateManager.GetPrevStateIsWallSlide())
            playerStateManager.GetRigidBody2D().velocity = new Vector2(playerStateManager.GetRigidBody2D().velocity.x, playerStateManager.GetSpeedY());
        else
        {
            if (playerStateManager.GetIsFacingRight())
            {
                playerStateManager.GetRigidBody2D().velocity = new Vector2(-1 * playerStateManager.GetWallJumpSpeedX(), playerStateManager.GetWallJumpSpeedY());
                //Vì lúc trượt tường thì chưa set lại IsFR nên ở đây bắt buộc phải nhân -1
                //Debug.Log("FR");
            }
            else
            {
                playerStateManager.GetRigidBody2D().velocity = new Vector2(playerStateManager.GetWallJumpSpeedX(), playerStateManager.GetWallJumpSpeedY());
                //Debug.Log("FL");
            }
        }
        playerStateManager.GetJumpSound().Play();
    }

    private void UpdateHorizontalLogic()
    {
        if (playerStateManager.GetDirX() != 0)
        {
            playerStateManager.GetRigidBody2D().velocity = new Vector2(playerStateManager.GetSpeedX() * playerStateManager.GetDirX(), playerStateManager.GetRigidBody2D().velocity.y);
        }
    }

    public override void FixedUpdate()
    {

    }

}
