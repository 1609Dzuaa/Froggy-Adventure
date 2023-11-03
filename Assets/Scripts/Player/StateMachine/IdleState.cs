using UnityEngine;

public class IdleState : BaseState
{
    public override void EnterState(BaseStateManager _baseStateManager)
    {
        if (_baseStateManager is PlayerStateManager)
        {
            playerStateManager = (PlayerStateManager)_baseStateManager;
            playerStateManager.GetAnimator().SetInteger("state", (int)EnumState.EState.idle);
            
            //State WS sẽ có đặc biệt chút là sẽ flip sprite ngược với isFacingRight
            //Ta sẽ check nếu biến bool prev State là WS thì flip ngược lại
            //và set lại biến đó sau khi flip xong 
            if(playerStateManager.GetPrevStateIsWallSlide())
                FlipSpriteAfterWallSlide();
            
            //Debug.Log("Idle"); Keep this, use for debugging change state
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
        if (playerStateManager.GetDirY() < 0) //Có thể để != 0 cho tổng quát ?
        {
            if (playerStateManager.GetIsOnGround())
                playerStateManager.ChangeState(playerStateManager.jumpState);
        }
        else if (!playerStateManager.GetIsOnGround())
        {
            //ĐK cũ có vấn đề: Lúc WS chạm đất đứng yên thì Idle 1 lúc r Fall r mới Idle
            //ĐK cũ: (playerStateManager.GetRigidBody2D().velocity.y < -0.1f)
            //ĐK mới: Nếu DirectionY == 0 tức ng chơi 0 tác động lên trục Y và
            //0 chạm đất thì chuyển qua Fall

            playerStateManager.ChangeState(playerStateManager.fallState);
        }
    }

    public override void FixedUpdate()
    {

    }

    private void FlipSpriteAfterWallSlide()
    {
        playerStateManager.FlippingSprite();
        playerStateManager.SetPrevStateIsWallSlide(false);
    }
}