﻿using Unity.VisualScripting;
using UnityEngine;

public class JumpState : BaseState
{
    public override void EnterState(BaseStateManager stateManager)
    {
        stateManager.GetAnimator().SetInteger("state", (int)EnumState.EState.jump);
        //playerController.GetJumpSound().Play();
    }

    public override void ExitState(BaseStateManager stateManager)
    {
        //Từ Jump -> Wall Slide tạm ổn
    }

    public override void UpdateState(BaseStateManager stateManager)
    {
        /*UpdateJumpLogic(stateManager, playerController);

        if (playerController.GetRigidbody2D().velocity.y < -0.1f)
            stateManager.ChangeState(stateManager.fallState);

        UpdateHorizontalLogic(stateManager, playerController);*/
    }

    void UpdateJumpLogic(BaseStateManager stateManager)
    {
        /*if (playerController.GetDirY() < 0)
        {
            if (playerController.GetIsOnGround())
            {
                playerController.GetRigidbody2D().velocity = new Vector2(playerController.GetRigidbody2D().velocity.x, playerController.GetvY());
                playerController.SetIsOnGround(false);
            }
        }*/
    }

    void UpdateHorizontalLogic(BaseStateManager stateManager)
    {
        /*if (playerController.GetDirX() != 0)
        {
            FlipSprite(stateManager, playerController);

            playerController.GetRigidbody2D().velocity = new Vector2(playerController.GetvX() * playerController.GetDirX(), playerController.GetRigidbody2D().velocity.y);
        }*/
    }

    public override void FixedUpdate(BaseStateManager stateManager)
    {

    }

    private void FlipSprite(BaseStateManager stateManager)
    {
        /*if (playerController.GetDirX() < 0)
            stateManager.GetSpriteRenderer().flipX = true;
        else
            stateManager.GetSpriteRenderer().flipX = false;
        
        //Hàm này dùng để lật sprite theo trục hoành*/
    }

    /*private void OnCollisionEnter2D(Collision collision)
    {
        if(collision.collider.CompareTag("Rock"))

    }*/

}
