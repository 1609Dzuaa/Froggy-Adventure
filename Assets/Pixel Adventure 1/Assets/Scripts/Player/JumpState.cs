using UnityEngine;

public class JumpState : BaseState
{
    public override void EnterState(StateManager stateManager)
    {
        stateManager.GetAnimator().SetInteger("state", (int)StateManager.EnumState.jump);
    }

    public override void ExitState(StateManager stateManager)
    {

    }

    public override void UpdateState(StateManager stateManager)
    {
        UpdateJumpLogic(stateManager);

        if (stateManager.getRigidbody2D().velocity.y < -0.1f)
            stateManager.ChangeState(stateManager.fallState);

        UpdateHorizontalLogic(stateManager);
    }

    void UpdateJumpLogic(StateManager stateManager)
    {
        if (stateManager.getDirY() < 0)
        {
            if (stateManager.getIsOnGround())
            {
                stateManager.getRigidbody2D().velocity = new Vector2(stateManager.getRigidbody2D().velocity.x, stateManager.getvY());
                stateManager.setIsOnGround(false);
            }
        }
    }

    void UpdateHorizontalLogic(StateManager stateManager)
    {
        if (stateManager.getDirX() != 0)
        {
            //Lật mặt
            if (stateManager.getDirX() < 0)
                stateManager.GetSpriteRenderer().flipX = true;
            else
                stateManager.GetSpriteRenderer().flipX = false;

            stateManager.getRigidbody2D().velocity = new Vector2(stateManager.getvX() * stateManager.getDirX(), stateManager.getRigidbody2D().velocity.y);
        }
    }

    public override void FixedUpdate(StateManager stateManager)
    {

    }

}
