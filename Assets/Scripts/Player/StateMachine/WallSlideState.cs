using System;
using UnityEngine;

public class WallSlideState : BaseState
{
    public override void EnterState(BaseStateManager stateManager)
    {
        if (stateManager is PlayerStateManager)
        {
            playerStateManager.GetAnimator().SetInteger("state", (int)EnumState.EState.wallSlide);
        }
        playerStateManager.GetRigidBody2D().gravityScale = 0.5f;
    }

    public override void ExitState()
    {
        playerStateManager.GetRigidBody2D().gravityScale = 1.7f;
    }

    public override void UpdateState()
    {
        UpdateHorizontalLogic();
        UpdateVerticalLogic();

        playerStateManager.GetRigidBody2D().gravityScale = 0.5f;
    }

    void UpdateHorizontalLogic()
    {
        
        //Lúc slide wall xuống thì:
        //nếu vY rất nhỏ thì change sang idle
        //nếu kh thì fall
    }

    void UpdateVerticalLogic()
    {
        //Hướng Y khác 0 tức là đang nhảy hoặc rơi
        /*if (playerController.GetDirY() < 0)
        {
            if (playerController.GetIsOnGround())
                stateManager.ChangeState(stateManager.jumpState);
        }
        else if (playerController.GetRigidbody2D().velocity.y < -0.1f)
        {
            stateManager.ChangeState(stateManager.fallState);
        }*/

    }

    public override void FixedUpdate()
    {
        /*if(Math.Abs(playerStateManager.GetRigidBody2D().velocity.y) < 0.1f)
        {
            playerStateManager.ChangeState(playerStateManager.idleState);
        }*/
    }
}