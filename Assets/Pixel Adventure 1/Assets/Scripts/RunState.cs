using UnityEngine;

public class RunState : BaseState
{
    public override void EnterState(StateManager stateManager)
    {
        stateManager.GetAnimator().SetInteger("state", (int)StateManager.EnumState.walk);
    }

    public override void ExitState(StateManager stateManager)
    {

    }

    public override void UpdateState(StateManager stateManager)
    {
        //UpdateHorizontalLogic(stateManager);

        UpdateVerticalLogic(stateManager);
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
        else //X-direction = 0
        {
            stateManager.ChangeState(stateManager.idleState);
        }
    }

    void UpdateVerticalLogic(StateManager stateManager)
    {
        //Hướng Y khác 0 tức là đang nhảy hoặc rơi

        if (stateManager.getDirY() < 0)
        {
            if (stateManager.getIsOnGround())
                stateManager.ChangeState(stateManager.jumpState);
        }
        else if (stateManager.getRigidbody2D().velocity.y < -0.1f)
        {
            stateManager.ChangeState(stateManager.fallState);
        }
    }

    public override void FixedUpdate(StateManager stateManager)
    {
        UpdateHorizontalLogic(stateManager);
    }
}
