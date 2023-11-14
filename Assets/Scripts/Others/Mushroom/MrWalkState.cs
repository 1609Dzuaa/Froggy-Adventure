using UnityEngine;

public class MrWalkState : BaseState
{
    private bool hasChangeState = false;
    private float entryTime; //đánh dấu thgian bắt đầu state
    public override void EnterState(BaseStateManager _baseStateManager)
    {
        if (_baseStateManager is MushroomStateManager)
        {
            mushroomStateManager = (MushroomStateManager)_baseStateManager;
            mushroomStateManager.GetAnimator().SetInteger("state", (int)EnumState.EMushroomState.walk);
            entryTime = Time.time;
            //Debug.Log("Walk"); 
        }
    }

    public override void ExitState()
    {
        hasChangeState = false;
    }

    public override void UpdateState()
    {
        if (mushroomStateManager.GetHasDetectedPlayer() && !hasChangeState)
        {
            hasChangeState = true;
            mushroomStateManager.Invoke("AllowRunFromPlayer", 0.2f);
        }
        else if (Time.time - entryTime >= mushroomStateManager.GetWalkDuration() && !hasChangeState && !mushroomStateManager.GetHasCollidedWall())
        {
            hasChangeState = true;
            mushroomStateManager.mrIdleState.SetCanRdDirection(true);
            mushroomStateManager.ChangeState(mushroomStateManager.mrIdleState);
        }
        else if(mushroomStateManager.GetHasCollidedWall())
        {
            hasChangeState = true;
            mushroomStateManager.ChangeState(mushroomStateManager.mrIdleState);
            mushroomStateManager.FlippingSprite();
        }
    }

    public override void FixedUpdate()
    {
        if (mushroomStateManager.GetIsFacingRight())
            mushroomStateManager.GetRigidBody2D().velocity = new Vector2(mushroomStateManager.GetWalkSpeed(), mushroomStateManager.GetRigidBody2D().velocity.y);
        else
            mushroomStateManager.GetRigidBody2D().velocity = new Vector2(-mushroomStateManager.GetWalkSpeed(), mushroomStateManager.GetRigidBody2D().velocity.y);
    }

}
