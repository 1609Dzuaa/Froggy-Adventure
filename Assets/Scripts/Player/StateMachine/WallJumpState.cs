using System;
using UnityEngine;

public class WallJumpState : BaseState
{
    public override void EnterState(BaseStateManager stateManager)
    {
        stateManager.GetAnimator().SetInteger("state", (int)EnumState.EState.walljump);
    }

    public override void ExitState(BaseStateManager stateManager)
    {

    }

    public override void UpdateState(BaseStateManager stateManager)
    {
        //UpdateHorizontalLogic(stateManager, playerController);

        //UpdateVerticalLogic(stateManager, playerController);
    }

    void UpdateHorizontalLogic(BaseStateManager stateManager)
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

    void UpdateVerticalLogic(BaseStateManager stateManager)
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

    public override void FixedUpdate(BaseStateManager stateManager)
    {
        /*if(Math.Abs(playerController.GetRigidbody2D().velocity.y) < 0.1f)
        {
            stateManager.ChangeState(stateManager.idleState);
        }*/
    }
}