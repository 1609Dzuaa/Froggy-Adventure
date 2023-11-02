using UnityEngine;

public class IdleState : BaseState
{
    public override void EnterState(BaseStateManager _baseStateManager)
    {
        if (_baseStateManager is PlayerStateManager)
        {
            playerStateManager = (PlayerStateManager)_baseStateManager;
            playerStateManager.GetAnimator().SetInteger("state", (int)EnumState.EState.idle);
            Debug.Log("Idle");
        }
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        UpdateHorizontalLogic();
        UpdateVerticalLogic();
    }

    void UpdateHorizontalLogic()
    {
        //Hướng X khác 0 tức là đang di chuyển || dash
        if (playerStateManager.GetDirX() != 0)
        {
            playerStateManager.ChangeState(playerStateManager.runState);
        }
    }

    void UpdateVerticalLogic()
    {
        //Hướng Y khác 0 tức là đang nhảy hoặc rơi
        if (playerStateManager.GetDirY() < 0)
        {
            if (playerStateManager.GetIsOnGround())
                playerStateManager.ChangeState(playerStateManager.jumpState);
        }
        else if (playerStateManager.GetRigidBody2D().velocity.y < -0.1f)
        {
            playerStateManager.ChangeState(playerStateManager.fallState);
        }
    }

    public override void FixedUpdate()
    {

    }
}