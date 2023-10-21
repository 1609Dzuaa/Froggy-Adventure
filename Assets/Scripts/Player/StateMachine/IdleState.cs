using UnityEngine;

public class IdleState : BaseState
{
    public override void EnterState(BaseStateManager _baseStateManager)
    {
        if (_baseStateManager is PlayerStateManager)
        {
            playerStateManager = (PlayerStateManager)_baseStateManager;
            /*if (playerStateManager.GetAnimator() == null)
            {
                Debug.Log("NULL here");
            }*/
            playerStateManager.GetAnimator().SetInteger("state", (int)EnumState.EState.idle);
        }
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        if(this.baseStateManager is PlayerStateManager) 
        {
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

    public override void FixedUpdate()
    {

    }
}