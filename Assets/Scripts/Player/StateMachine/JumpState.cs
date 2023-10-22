using Unity.VisualScripting;
using UnityEngine;

public class JumpState : BaseState
{
    public override void EnterState(BaseStateManager stateManager)
    {
        if (stateManager is PlayerStateManager)
        {
            playerStateManager.GetAnimator().SetInteger("state", (int)EnumState.EState.jump);
        }
        playerStateManager.GetJumpSound().Play();
    }

    public override void ExitState()
    {
        //Từ Jump -> Wall Slide tạm ổn
    }

    public override void UpdateState()
    {
        UpdateJumpLogic();

        if (playerStateManager.GetRigidBody2D().velocity.y < -0.1f)
            playerStateManager.ChangeState(playerStateManager.fallState);

        UpdateHorizontalLogic();
    }

    void UpdateJumpLogic()
    {
        if (playerStateManager.GetDirY() < 0)
        {
            if (playerStateManager.GetIsOnGround())
            {
                playerStateManager.GetRigidBody2D().velocity = new Vector2(playerStateManager.GetRigidBody2D().velocity.x, playerStateManager.GetvY());
                playerStateManager.SetIsOnGround(false);
            }
        }
    }

    void UpdateHorizontalLogic()
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
