using UnityEngine;

public class IdleState : BaseState
{
    public override void EnterState(StateManager stateManager, PlayerController playerController)
    {
        stateManager.GetAnimator().SetInteger("state", (int)StateManager.EnumState.idle);
    }

    public override void ExitState(StateManager stateManager, PlayerController playerController)
    {

    }

    public override void UpdateState(StateManager stateManager, PlayerController playerController)
    {
        UpdateHorizontalLogic(stateManager, playerController);

        UpdateVerticalLogic(stateManager, playerController);
    }

    void UpdateHorizontalLogic(StateManager stateManager, PlayerController playerController)
    {
        //Hướng X khác 0 tức là đang di chuyển
        if (playerController.GetDirX() != 0)
        {
            stateManager.ChangeState(stateManager.runState);
        }
    }

    void UpdateVerticalLogic(StateManager stateManager, PlayerController playerController)
    {
        //Hướng Y khác 0 tức là đang nhảy hoặc rơi
        if (playerController.GetDirY() < 0)
        {
            if (playerController.GetIsOnGround())
                stateManager.ChangeState(stateManager.jumpState);
        }
        else if (playerController.GetRigidbody2D().velocity.y < -0.1f)
        {
            stateManager.ChangeState(stateManager.fallState);
        }
    }

    public override void FixedUpdate(StateManager stateManager, PlayerController playerController)
    {

    }
}