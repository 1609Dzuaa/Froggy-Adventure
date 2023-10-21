using System;
using UnityEngine;

public class FallState : BaseState
{
    public override void EnterState(BaseStateManager stateManager)
    {
        if (stateManager is PlayerStateManager)
        {
            PlayerStateManager playerStateManager = (PlayerStateManager)baseStateManager;
            playerStateManager.GetAnimator().SetInteger("state", (int)EnumState.EState.fall);
        }
    }

    public override void ExitState()
    {
        //từ fall sang run đang có chút vấn đề
    }

    public override void UpdateState()
    {
        //Because the velocity value will not always exactly equal 0
        //So we check does it greater or smaller than a very small value

        if(baseStateManager is PlayerStateManager)
        {
            PlayerStateManager playerStateManager = (PlayerStateManager)baseStateManager;
            //Nếu vận tốc 2 trục rất nhỏ thì coi như đang Idle
            if (Math.Abs(playerStateManager.GetRigidBody2D().velocity.x) < 0.1f && Math.Abs(playerStateManager.GetRigidBody2D().velocity.y) < 0.1f)
                playerStateManager.ChangeState(playerStateManager.idleState);

            //Nếu vận tốc trục x lớn hơn .1f và trục y rất nhỏ thì
            //chuyển sang state Walk
            if (Math.Abs(playerStateManager.GetRigidBody2D().velocity.x) > 0.1f && Math.Abs(playerStateManager.GetRigidBody2D().velocity.y) < 0.1f)
                baseStateManager.ChangeState(playerStateManager.runState);
        }

        UpdateHorizontalLogic();
    }

    void UpdateHorizontalLogic()
    {
        if(baseStateManager is PlayerStateManager)
        {
            PlayerStateManager playerStateManager = (PlayerStateManager)baseStateManager;
            if (playerStateManager.GetDirX() != 0)
            {
                //Lật mặt
                if (playerStateManager.GetDirX() < 0)
                    baseStateManager.GetSpriteRenderer().flipX = true;
                else
                    baseStateManager.GetSpriteRenderer().flipX = false;

                //Prob here
                //playerStateManager.GetRigidBody2D().velocity = new Vector2(playerController.GetvX() * playerController.GetDirX(), playerController.GetRigidbody2D().velocity.y);
            }
        }
    }

    public override void FixedUpdate()
    {

    }
}
