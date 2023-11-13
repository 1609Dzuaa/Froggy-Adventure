using UnityEngine;

public class RunState : BaseState
{
    public override void EnterState(BaseStateManager _baseStateManager)
    {
        if (_baseStateManager is PlayerStateManager)
        {
            playerStateManager.GetAnimator().SetInteger("state", (int)EnumState.EState.run);
            playerStateManager.GetDustPS().Play();
            //Debug.Log("Run");
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
        if (playerStateManager.GetDirX() != 0)
        {
            playerStateManager.GetRigidBody2D().velocity = new Vector2(playerStateManager.GetSpeedX() * playerStateManager.GetDirX(), playerStateManager.GetRigidBody2D().velocity.y);
        }
        else //X-direction = 0
        {
            playerStateManager.ChangeState(playerStateManager.idleState);
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
        UpdateHorizontalLogic();
    }
}
