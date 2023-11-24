using UnityEngine;

public class BunnyPatrolState : BunnyBaseState
{
    private float entryTime;
    private int isLeftOrRight = 0; //0: Left; 1: Right
    private bool hasChangedState = false;

    public override void EnterState(BunnyStateManager bunnyStateManager)
    {
        base.EnterState(bunnyStateManager);
        _bunnyStateManager.GetAnimator().SetInteger("state", (int)EnumState.EBunnyState.patrol);
        entryTime = Time.time;
        isLeftOrRight = Random.Range(0, 2);
        if (isLeftOrRight > 0 && !_bunnyStateManager.GetIsFacingRight())
            _bunnyStateManager.FlippingSprite();
        else if (isLeftOrRight == 0 && _bunnyStateManager.GetIsFacingRight())
            _bunnyStateManager.FlippingSprite();
    }

    public override void ExitState() { hasChangedState = false; }

    public override void Update() 
    {
        if (Time.time - entryTime > _bunnyStateManager.GetPatrolTime() && !hasChangedState)
        {
            hasChangedState = true;
            _bunnyStateManager.ChangeState(_bunnyStateManager.bunnyIdleState);
        }
        else if (_bunnyStateManager.GetHasDetectPlayer() && !hasChangedState)
        {
            hasChangedState = true;
            _bunnyStateManager.Invoke("JumpDelay", _bunnyStateManager.GetJumpDelay());
        }
        else if (_bunnyStateManager.GetHasCollidedWall())
            _bunnyStateManager.FlippingSprite();
    }

    public override void FixedUpdate() 
    {
        _bunnyStateManager.GetRigidBody2D().velocity = -1 * _bunnyStateManager.transform.right * _bunnyStateManager.GetPatrolSpeed();
    }

}
