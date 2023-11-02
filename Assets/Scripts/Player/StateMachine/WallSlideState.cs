using System;
using UnityEngine;

public class WallSlideState : BaseState
{
    public override void EnterState(BaseStateManager stateManager)
    {
        if (stateManager is PlayerStateManager)
        {
            playerStateManager.GetAnimator().SetInteger("state", (int)EnumState.EState.wallSlide);
            Debug.Log("WS");
        }
        //playerStateManager.GetRigidBody2D().gravityScale = 0.5f;
        //Flip sprite khi chuyển từ state này sang state bất kì
        //Theo đúng chiều của nhân vật khi đang slide
        //Collide trái => mặt hướng trái

        //Đang có bug Jump trên wall dù nhấn D thì lại change sang run trên wall ??
        //Đặt Debug thấy Jump => WS => Idle => Run
        //=>Do ĐK
    }

    public override void ExitState()
    {
        //playerStateManager.GetRigidBody2D().gravityScale = 1.7f;
    }

    public override void UpdateState()
    {
        UpdateHorizontalLogic();
        UpdateVerticalLogic();
    }

    void UpdateHorizontalLogic()
    {
        //muốn fall khi đang slide thì cân nhắc Collision Exit, Trigger Exit ?
        if (playerStateManager.GetDirX() != 0)
        {
            playerStateManager.GetRigidBody2D().velocity = new Vector2(playerStateManager.GetvX() * playerStateManager.GetDirX(), playerStateManager.GetRigidBody2D().velocity.y);

            //if (playerStateManager.GetLeft() * playerStateManager.GetDirX() > 0 || playerStateManager.GetRight() * playerStateManager.GetDirX() > 0)
            //playerStateManager.ChangeState(playerStateManager.fallState);
        }
    }

    void UpdateVerticalLogic()
    {
        //Lúc slide wall xuống thì:
        //nếu chạm đất thì change sang idle
        //nếu bấm S thì change sang nhảy
        //Prob here
        if (playerStateManager.GetIsOnGround())
            playerStateManager.ChangeState(playerStateManager.idleState);
        if (Input.GetKeyDown(KeyCode.S))
            playerStateManager.ChangeState(playerStateManager.jumpState);

        playerStateManager.GetRigidBody2D().velocity = new Vector2(playerStateManager.GetRigidBody2D().velocity.x, -1 * playerStateManager.GetWallSlideSpeed());
    }

    private void FlippingSprite()
    {

    }

    public override void FixedUpdate()
    {
        
    }
}