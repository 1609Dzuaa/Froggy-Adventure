using System;
using UnityEngine;

public class FallState : BaseState
{
    public override void EnterState(BaseStateManager _baseStateManager)
    {
        if (_baseStateManager is PlayerStateManager)
        {
            playerStateManager.GetAnimator().SetInteger("state", (int)EnumState.EState.fall);

            if (playerStateManager.GetPrevStateIsWallSlide())
                playerStateManager.FlipSpriteAfterWallSlide();
            //Debug.Log("Fall");
        }
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        UpdateHorizontalLogic();
        //Because the velocity value will not always exactly equal 0
        //So we check does it greater or smaller than a very small value

        //Nếu vận tốc 2 trục rất nhỏ thì coi như đang Idle
        if (Math.Abs(playerStateManager.GetRigidBody2D().velocity.x) < 0.1f && Math.Abs(playerStateManager.GetRigidBody2D().velocity.y) < 0.1f)
            playerStateManager.ChangeState(playerStateManager.idleState);

        //Nếu vận tốc trục x lớn hơn .1f và trục y rất nhỏ
        //và đang OnGround thì
        //chuyển sang state Run
        //Prob here ?
        if (Math.Abs(playerStateManager.GetRigidBody2D().velocity.x) > 0.1f && Math.Abs(playerStateManager.GetRigidBody2D().velocity.y) < 0.1f && playerStateManager.GetIsOnGround())
            playerStateManager.ChangeState(playerStateManager.runState);

        if (playerStateManager.GetIsWallTouch() && !playerStateManager.GetIsOnGround())
            playerStateManager.ChangeState(playerStateManager.wallSlideState);

        //Cho phép lúc Fall có thể Double Jump đc
        if (Input.GetKeyDown(KeyCode.S) && !playerStateManager.GetHasDbJump())
            playerStateManager.ChangeState(playerStateManager.doubleJumpState);
    }

    void UpdateHorizontalLogic()
    {
        //if (baseStateManager is PlayerStateManager)
        //{
            //Prob here
            playerStateManager.GetRigidBody2D().velocity = new Vector2(playerStateManager.GetvX() * playerStateManager.GetDirX(), playerStateManager.GetRigidBody2D().velocity.y);
        //}
    }

    public override void FixedUpdate()
    {

    }
}
