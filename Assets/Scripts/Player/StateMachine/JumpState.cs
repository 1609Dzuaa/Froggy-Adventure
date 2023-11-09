using Unity.VisualScripting;
using UnityEngine;

public class JumpState : BaseState
{
    //Considering Coyote Time
    private bool hasChangedState = false;
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
            //Debug.Log("Jump");
            //cung` dau: WS, JUMP, JUMP, WS
            hasChangedState = false;
        }
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        UpdateJumpLogic();
        if (hasChangedState) 
            return;
        UpdateHorizontalLogic();
    }

    private void UpdateJumpLogic()
    {
        //Press S While Jump => Double Jump
        if (Input.GetKeyDown(KeyCode.S))
        {
            playerStateManager.ChangeState(playerStateManager.doubleJumpState);
            hasChangedState = true;
        }
        else if (playerStateManager.GetRigidBody2D().velocity.y < -0.1f)
        {
            //Debug.Log("Change here: " + playerStateManager.GetRigidBody2D().velocity.y);
            playerStateManager.ChangeState(playerStateManager.fallState);
            hasChangedState = true;
        }
        //Still Prob Here
        else if (playerStateManager.GetIsWallTouch() && !playerStateManager.GetIsOnGround())
        {
            playerStateManager.ChangeState(playerStateManager.wallSlideState);
            hasChangedState = true;
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
                //Debug.Log("vX, vY: " + playerStateManager.GetRigidBody2D().velocity.x +", "+ playerStateManager.GetRigidBody2D().velocity.y);
            }
        }
        playerStateManager.GetJumpSound().Play();
    }

    private void UpdateHorizontalLogic()
    {
        //Prob here:
        //Khi bay nhưng điều hướng A||D thì lại chui vào đây mất
        //Do hàm FLip after WS khiến WallCheck bị lật ngược lại dẫn đến thế này
        if (playerStateManager.GetDirX() != 0 )//&& !playerStateManager.GetIsWallTouch())
        {
            //Debug.Log("Here");
            playerStateManager.GetRigidBody2D().velocity = new Vector3(playerStateManager.GetSpeedX() * playerStateManager.GetDirX(), playerStateManager.GetRigidBody2D().velocity.y);
        }
    }

    public override void FixedUpdate()
    {

    }

}
