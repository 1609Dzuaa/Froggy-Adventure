using System;
using UnityEngine;

public class WallJumpState : BaseState
{
    public override void EnterState(BaseStateManager stateManager)
    {
        if (stateManager is PlayerStateManager)
        {
            PlayerStateManager playerStateManager = (PlayerStateManager)baseStateManager;
            playerStateManager.GetAnimator().SetInteger("state", (int)EnumState.EState.walljump);
        }
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        //UpdateHorizontalLogic(stateManager, playerController);

        //UpdateVerticalLogic(stateManager, playerController);
    }

    void UpdateHorizontalLogic()
    {
        /*Hướng X khác 0 tức là đang di chuyển
        if (playerController.GetDirX() != 0)
        {
            stateManager.ChangeState(stateManager.runState);
        }*/
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
        /*if(Math.Abs(playerController.GetRigidbody2D().velocity.y) < 0.1f)
        {
            stateManager.ChangeState(stateManager.idleState);
        }*/
    }
}