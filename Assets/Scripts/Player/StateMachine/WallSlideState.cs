using System;
using UnityEngine;

public class WallSlideState : BaseState
{
    public override void EnterState(BaseStateManager stateManager)
    {
        if (stateManager is PlayerStateManager)
        {
            playerStateManager.GetAnimator().SetInteger("state", (int)EnumState.EState.wallSlide);
            //Debug.Log("WS");
        }
        //playerStateManager.GetRigidBody2D().gravityScale = 0.5f;
        //Flip sprite khi chuyển từ state này sang state bất kì
        //Theo đúng chiều của nhân vật khi đang slide
        //Collide trái => mặt hướng trái

        //=>Do ĐK
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
            //Hiểu đơn giản là tích 2 vector của directionX và facingRight trái dấu
            //thì => fall
            if (!playerStateManager.GetIsFacingRight() && playerStateManager.GetDirX() > 0
                || playerStateManager.GetIsFacingRight() && playerStateManager.GetDirX() < 0)
                //|| !playerStateManager.GetIsOnGround() && !playerStateManager.GetIsWallTouch())
                playerStateManager.ChangeState(playerStateManager.fallState);
        }
    }

    void UpdateVerticalLogic()
    {
        //Lúc slide wall xuống thì:
        //nếu chạm đất thì change sang idle
        //nếu bấm S thì change sang nhảy
        //Prob here
        if (playerStateManager.GetIsOnGround() && playerStateManager.GetDirX() == 0)
            playerStateManager.ChangeState(playerStateManager.idleState);
        if (Input.GetKeyDown(KeyCode.S))
            playerStateManager.ChangeState(playerStateManager.jumpState);

        playerStateManager.GetRigidBody2D().velocity = new Vector2(playerStateManager.GetRigidBody2D().velocity.x, -1 * playerStateManager.GetWallSlideSpeed());
    }

    public override void FixedUpdate()
    {
        
    }
}