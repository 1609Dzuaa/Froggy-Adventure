using System;
using UnityEngine;

public class WallSlideState : BaseState
{
    private bool hasChangedState = false;
    public override void EnterState(BaseStateManager stateManager)
    {
        if (stateManager is PlayerStateManager)
        {
            playerStateManager.GetAnimator().SetInteger("state", (int)EnumState.EState.wallSlide);
            //Debug.Log("WS");
        }
        //Flip sprite khi chuyển từ state này sang state bất kì
        //Theo đúng chiều của nhân vật khi đang slide
        //Lỗi logic 1 chút ở đây là khi WS thì vector isFacingRight
        //vẫn là vector của state trước state WS này
        //Sau này có lỗi WS nhớ vào đây check 1st.
        hasChangedState = false;
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        UpdateHorizontalLogic(); //Xong đây nó lại set Velo ở else khiến cho 0 bay đc
        if (hasChangedState)
        {
            //Debug.Log("Changed, return");
            return;
        }
        UpdateVerticalLogic();
    }

    void UpdateHorizontalLogic()
    {
        if (playerStateManager.GetDirX() != 0)
        {
            //Hiểu đơn giản là 2 vector của directionX và facingRight trái dấu nhau
            //thì => fall
            if (!playerStateManager.GetIsFacingRight() && playerStateManager.GetDirX() > 0
                || playerStateManager.GetIsFacingRight() && playerStateManager.GetDirX() < 0)
            {
                playerStateManager.ChangeState(playerStateManager.fallState);
                hasChangedState = true;
            }
            else if (!playerStateManager.GetIsFacingRight() && playerStateManager.GetDirX() < 0 && Input.GetKeyDown(KeyCode.S)
                || playerStateManager.GetIsFacingRight() && playerStateManager.GetDirX() > 0 && Input.GetKeyDown(KeyCode.S))
            {
                playerStateManager.ChangeState(playerStateManager.jumpState);
                hasChangedState = true;
            }
        }
    }

    void UpdateVerticalLogic()
    {
        //Lúc slide wall xuống thì:
        //nếu chạm đất và 0 move thì change sang idle
        //nếu bấm S thì change sang nhảy
        //nếu slide hết tường mà vẫn trên không thì fall
        //nếu 0 thuộc các TH trên thì bơm nó vận tốc trục Y để slide như bthg

        //Done: Quên mất lồng else vào không thì nó chạy hết :)), kh muốn lồng else 
        //thì return sau khi changeState luôn 
        if (playerStateManager.GetIsOnGround() && playerStateManager.GetDirX() == 0)
        {
            playerStateManager.ChangeState(playerStateManager.idleState);
            hasChangedState = true;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            playerStateManager.ChangeState(playerStateManager.jumpState);
            //Nhảy trên tường gặp vấn đề cuối:
            //Giữ A/D sau khi trượt tường trái/phải 1 lúc sau đó bấm S thì 0 nhảy đc
            //Thả A/D sau khi trượt tường trái/phải 1 lúc sau đó bấm S thì mới nhảy đc
        }
        else if (!playerStateManager.GetIsOnGround() && !playerStateManager.GetIsWallTouch()
                && playerStateManager.GetRigidBody2D().velocity.y < -.1f)
        {
            playerStateManager.ChangeState(playerStateManager.fallState); //Trường hợp trượt hết tường mà vẫn fall
            hasChangedState = true;
        }
        else if(!hasChangedState)//Prob here too: Vì bấm S ở hàm Horizontal rồi,
             //nó change state rồi nhưng xuống đây nó lại set lại v => dẫn đến rối
            playerStateManager.GetRigidBody2D().velocity = new Vector2(playerStateManager.GetRigidBody2D().velocity.x, -1 * playerStateManager.GetWallSlideSpeed());
    }

    public override void FixedUpdate()
    {
        
    }
}