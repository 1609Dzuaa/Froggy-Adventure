using UnityEngine;

public class RunState : BaseState
{
    public override void EnterState(StateManager stateManager, PlayerController playerController)
    {
        stateManager.GetAnimator().SetInteger("state", (int)StateManager.EnumState.walk);
    }

    public override void ExitState(StateManager stateManager, PlayerController playerController)
    {

    }

    public override void UpdateState(StateManager stateManager, PlayerController playerController)
    {
        //UpdateHorizontalLogic(stateManager);

        UpdateVerticalLogic(stateManager, playerController);
    }

    void UpdateHorizontalLogic(StateManager stateManager, PlayerController playerController)
    {
        if (playerController.GetDirX() != 0)
        {
            //Lật mặt
            if (playerController.GetDirX() < 0)
                stateManager.GetSpriteRenderer().flipX = true;
            else
                stateManager.GetSpriteRenderer().flipX = false;

            playerController.GetRigidbody2D().velocity = new Vector2(playerController.GetvX() * playerController.GetDirX(), playerController.GetRigidbody2D().velocity.y);
            //playerController.transform.position += new Vector3(playerController.GetvX() * playerController.GetDirX(), playerController.transform.position.y) * Time.deltaTime;
        }
        else //X-direction = 0
        {
            stateManager.ChangeState(stateManager.idleState);
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
        UpdateHorizontalLogic(stateManager, playerController);
    }
}
