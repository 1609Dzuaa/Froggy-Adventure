using UnityEngine;

public class MrWalkState : MrBaseState
{
    private bool hasChangeState = false;
    private float entryTime; //đánh dấu thgian bắt đầu state
    public override void EnterState(MrStateManager mrStateManager)
    {
        base.EnterState(mrStateManager);
        _mrStateManager.GetAnimator().SetInteger("state", (int)EnumState.EMushroomState.walk);
        entryTime = Time.time;
        //Debug.Log("Walk"); 
    }

    public override void ExitState()
    {
        hasChangeState = false;
    }

    public override void Update()
    {
        if (_mrStateManager.GetHasDetectedPlayer() && !hasChangeState)
        {
            hasChangeState = true;
            _mrStateManager.Invoke("AllowRunFromPlayer", _mrStateManager.GetRunDelay());
        }
        else if (Time.time - entryTime >= _mrStateManager.GetWalkDuration() && !hasChangeState && !_mrStateManager.GetHasCollidedWall())
        {
            hasChangeState = true;
            _mrStateManager.mrIdleState.SetCanRdDirection(true);
            _mrStateManager.ChangeState(_mrStateManager.mrIdleState);
        }
        else if(_mrStateManager.GetHasCollidedWall())
        {
            //still prob still here
            hasChangeState = true;
            _mrStateManager.ChangeState(_mrStateManager.mrIdleState);
            _mrStateManager.FlippingSprite();
        }
    }

    public override void FixedUpdate()
    {
        if (_mrStateManager.GetIsFacingRight())
            _mrStateManager.GetRigidBody2D().velocity = new Vector2(_mrStateManager.GetWalkSpeed(), _mrStateManager.GetRigidBody2D().velocity.y);
        else
            _mrStateManager.GetRigidBody2D().velocity = new Vector2(-_mrStateManager.GetWalkSpeed(), _mrStateManager.GetRigidBody2D().velocity.y);
    }

}
