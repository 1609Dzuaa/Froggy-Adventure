using UnityEngine;

public class DashState : BaseState
{
    float dash_start = 0.0f;
    //[SerializeField] private float dash_speed = 10.0f;

    public override void EnterState(BaseStateManager stateManager)
    {
        dash_start = Time.time;
        if (stateManager is PlayerStateManager)
        {
            PlayerStateManager playerStateManager = (PlayerStateManager)baseStateManager;
            playerStateManager.GetAnimator().SetInteger("state", (int)EnumState.EState.dash);
        }
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        //UpdateHorizontalLogic(stateManager, playerController);

        //UpdateVerticalLogic(stateManager, playerController);
    }

    /*void UpdateHorizontalLogic(StateManager stateManager, PlayerController playerController)
    {
        //Hướng X khác 0 tức là đang di chuyển || dash
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
    }*/

    public override void FixedUpdate()
    {
        //if (dash_start + Time.deltaTime <= 2.0f) //prob here
            //playerController.GetRigidbody2D().velocity = new Vector2(dash_speed, playerController.GetRigidbody2D().velocity.y);
    }
}