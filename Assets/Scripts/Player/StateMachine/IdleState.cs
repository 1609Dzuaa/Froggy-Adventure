using UnityEngine;

public class IdleState : BaseState
{
    public override void EnterState(BaseStateManager stateManager)
    {
        if(stateManager is PlayerStateManager)
        {
            stateManager.GetAnimator().SetInteger("state", (int)EnumState.EState.idle);
        }
    }

    public override void ExitState(BaseStateManager stateManager)
    {

    }

    public override void UpdateState(BaseStateManager stateManager)
    {
        if(stateManager is PlayerStateManager) 
        {
            PlayerStateManager playerStateManager = (PlayerStateManager)stateManager;
            UpdateHorizontalLogic(playerStateManager);
            UpdateVerticalLogic(playerStateManager);
        }
    }

    void UpdateHorizontalLogic(PlayerStateManager player_StateManager)
    {
        //Hướng X khác 0 tức là đang di chuyển || dash
        if (player_StateManager.GetDirX() != 0)
        {
            player_StateManager.ChangeState(player_StateManager.runState);
        }
    }

    void UpdateVerticalLogic(PlayerStateManager stateManager)
    {
        //Hướng Y khác 0 tức là đang nhảy hoặc rơi
        if (stateManager.GetDirY() < 0)
        {
            if (stateManager.GetIsOnGround())
                stateManager.ChangeState(stateManager.jumpState);
        }
        else if (stateManager.GetRigidBody2D().velocity.y < -0.1f)
        {
            stateManager.ChangeState(stateManager.fallState);
        }
    }

    public override void FixedUpdate(BaseStateManager stateManager)
    {

    }
}