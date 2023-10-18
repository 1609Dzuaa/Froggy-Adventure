using UnityEngine;

public class RunState : BaseState
{
    public override void EnterState(BaseStateManager stateManager)
    {
        stateManager.GetAnimator().SetInteger("state", (int)EnumState.EState.walk);
    }

    public override void ExitState(BaseStateManager stateManager)
    {

    }

    public override void UpdateState(BaseStateManager stateManager)
    {
        //UpdateHorizontalLogic(stateManager);

        //UpdateVerticalLogic(stateManager, playerController);
    }

    void UpdateHorizontalLogic(BaseStateManager stateManager)
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

    void UpdateVerticalLogic(BaseStateManager stateManager)
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

    public override void FixedUpdate(BaseStateManager stateManager)
    {
        //UpdateHorizontalLogic(stateManager, playerController);
    }
}
