using UnityEngine;

public class BunnyIdleState : BunnyBaseState
{
    private bool hasChangedState = false;
    private float entryTime;

    public override void EnterState(BunnyStateManager bunnyStateManager)
    {
        base.EnterState(bunnyStateManager);
        _bunnyStateManager.GetAnimator().SetInteger("state", (int)EnumState.EBunnyState.idle);
        _bunnyStateManager.GetRigidBody2D().velocity = Vector2.zero;
        entryTime = Time.time;
    }

    public override void ExitState() { hasChangedState = false; }

    public override void Update() 
    {
        if (_bunnyStateManager.GetHasDetectPlayer() && !hasChangedState)
        {
            hasChangedState = true;
            _bunnyStateManager.Invoke("JumpDelay", _bunnyStateManager.GetJumpDelay());
        }
        else if(Time.time - entryTime >= _bunnyStateManager.GetRestTime())
        {
            hasChangedState = true;
            _bunnyStateManager.ChangeState(_bunnyStateManager.bunnyPatrolState);
        }
    }

    public override void FixedUpdate() { }

}
