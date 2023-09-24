using System;
using UnityEngine;

public class FallState : BaseState
{
    public override void EnterState(StateManager stateManager)
    {
        stateManager.GetAnimator().SetInteger("state", (int)StateManager.EnumState.fall);
    }

    public override void ExitState(StateManager stateManager)
    {

    }

    public override void UpdateState(StateManager stateManager)
    {
        //Because the velocity value will not always exactly equal 0
        //So we check does it greater or smaller than a very small value

        //Nếu vận tốc 2 trục rất nhỏ thì coi như đang Idle
        if (Math.Abs(stateManager.getRigidbody2D().velocity.x) < 0.1f && Math.Abs(stateManager.getRigidbody2D().velocity.y) < 0.1f)
            stateManager.ChangeState(stateManager.idleState);

        //Nếu vận tốc trục x lớn hơn .1f và trục y rất nhỏ thì
        //chuyển sang state Walk
        if (Math.Abs(stateManager.getRigidbody2D().velocity.x) > 0.1f && Math.Abs(stateManager.getRigidbody2D().velocity.y) < 0.1f)
            stateManager.ChangeState(stateManager.runState);

        UpdateHorizontalLogic(stateManager);
    }

    void UpdateHorizontalLogic(StateManager stateManager)
    {
        if (stateManager.getDirX() != 0)
        {
            //Lật mặt
            if (stateManager.getDirX() < 0)
                stateManager.GetSpriteRenderer().flipX = true;
            else
                stateManager.GetSpriteRenderer().flipX = false;

            stateManager.getRigidbody2D().velocity = new Vector2(stateManager.getvX() * stateManager.getDirX(), stateManager.getRigidbody2D().velocity.y);
        }
    }

    public override void FixedUpdate(StateManager stateManager)
    {

    }
}
