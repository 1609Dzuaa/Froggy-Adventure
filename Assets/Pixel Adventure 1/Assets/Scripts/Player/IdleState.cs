using UnityEngine;

public class IdleState : BaseState
{
    public override void EnterState(StateManager stateManager)
    {
        stateManager.GetAnimator().SetInteger("state", (int)StateManager.EnumState.idle);
    }

    public override void ExitState(StateManager stateManager)
    {

    }

    public override void UpdateState(StateManager stateManager)
    {
        UpdateHorizontalLogic(stateManager);

        UpdateVerticalLogic(stateManager);
    }

    void UpdateHorizontalLogic(StateManager stateManager)
    {
        //Hướng X khác 0 tức là đang di chuyển
        if (stateManager.getDirX() != 0)
        {
            stateManager.ChangeState(stateManager.runState);
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

    }
}