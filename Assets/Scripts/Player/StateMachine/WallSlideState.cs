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
    }

    void UpdateHorizontalLogic()
    {
        //muốn fall khi đang slide thì cân nhắc Collision Exit, Trigger Exit ?
        if (playerStateManager.GetDirX() != 0)
        {
            //playerStateManager.FlippingSprite();

            playerStateManager.GetRigidBody2D().velocity = new Vector2(playerStateManager.GetvX() * playerStateManager.GetDirX(), playerStateManager.GetRigidBody2D().velocity.y);
            if (playerStateManager.GetLeft() * playerStateManager.GetDirX() > 0 || playerStateManager.GetRight() * playerStateManager.GetDirX() > 0)
                playerStateManager.ChangeState(playerStateManager.fallState);
        }
    }

    void UpdateVerticalLogic()
    {
        //Lúc slide wall xuống thì:
        //nếu chạm đất thì change sang idle
        //nếu bấm S thì change sang nhảy
        if (playerStateManager.GetIsOnGround())
            playerStateManager.ChangeState(playerStateManager.idleState);
        if (Input.GetKeyDown(KeyCode.S))
            playerStateManager.ChangeState(playerStateManager.jumpState);
    }

    public override void FixedUpdate()
    {
        /*if(Math.Abs(playerStateManager.GetRigidBody2D().velocity.y) < 0.1f)
        {
            playerStateManager.ChangeState(playerStateManager.idleState);
        }*/
    }
}