using UnityEngine;

public class RunState : BaseState
{
    public override void EnterState(BaseStateManager stateManager)
    {
        if (stateManager is PlayerStateManager)
        {
            PlayerStateManager playerStateManager = (PlayerStateManager)baseStateManager;
            playerStateManager.GetAnimator().SetInteger("state", (int)EnumState.EState.walk);
        }
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        //UpdateHorizontalLogic(stateManager);

        //UpdateVerticalLogic(stateManager, playerController);
    }

    void UpdateHorizontalLogic()
    {
        /*if (playerController.GetDirX() != 0)
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
        }*/
    }

    void UpdateVerticalLogic()
    {
        //Hướng Y khác 0 tức là đang nhảy hoặc rơi

        /*if (playerController.GetDirY() < 0)
        {
            if (playerController.GetIsOnGround())
                stateManager.ChangeState(stateManager.jumpState);
        }
        else if (playerController.GetRigidbody2D().velocity.y < -0.1f)
        {
            stateManager.ChangeState(stateManager.fallState);
        }*/
    }

    public override void FixedUpdate()
    {
        //UpdateHorizontalLogic(stateManager, playerController);
    }
}
