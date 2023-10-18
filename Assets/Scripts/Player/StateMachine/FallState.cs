using System;
using UnityEngine;

public class FallState : BaseState
{
    public override void EnterState(BaseStateManager stateManager)
    {
        stateManager.GetAnimator().SetInteger("state", (int)EnumState.EState.fall);
    }

    public override void ExitState(BaseStateManager stateManager)
    {
        //từ fall sang run đang có chút vấn đề
    }

    public override void UpdateState(BaseStateManager stateManager)
    {
        //Because the velocity value will not always exactly equal 0
        //So we check does it greater or smaller than a very small value

        //Nếu vận tốc 2 trục rất nhỏ thì coi như đang Idle
        /*if (Math.Abs(stateManager.GetRigidbody2D().velocity.x) < 0.1f && Math.Abs(playerController.GetRigidbody2D().velocity.y) < 0.1f)
            stateManager.ChangeState(stateManager.idleState);

        //Nếu vận tốc trục x lớn hơn .1f và trục y rất nhỏ thì
        //chuyển sang state Walk
        if (Math.Abs(playerController.GetRigidbody2D().velocity.x) > 0.1f && Math.Abs(playerController.GetRigidbody2D().velocity.y) < 0.1f)
            stateManager.ChangeState(stateManager.runState);

        UpdateHorizontalLogic(stateManager, playerController);*/
    }

    void UpdateHorizontalLogic(BaseStateManager stateManager)
    {
        /*if (playerController.GetDirX() != 0)
        {
            //Lật mặt
            if (playerController.GetDirX() < 0)
                stateManager.GetSpriteRenderer().flipX = true;
            else
                stateManager.GetSpriteRenderer().flipX = false;

            playerController.GetRigidbody2D().velocity = new Vector2(playerController.GetvX() * playerController.GetDirX(), playerController.GetRigidbody2D().velocity.y);
        }*/
    }

    public override void FixedUpdate(BaseStateManager stateManager)
    {

    }
}
