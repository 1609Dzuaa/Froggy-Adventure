using UnityEngine;

public class RhinoRunState : RhinoBaseState
{
    public override void EnterState(RhinoStateManager rhinoStateManager)
    {
        base.EnterState(rhinoStateManager);
        _rhinoStateManager.GetAnimator().SetInteger("state", (int)EnumState.ERhinoState.run);
        //Dùng Instantiate tạo Prefab Warning
        rhinoStateManager.SpawnWarning();

        //Debug.Log("Run");
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        if (_rhinoStateManager.GetHasCollidedWall())
            _rhinoStateManager.ChangeState(_rhinoStateManager.rhinoWallHitState);
    }

    void UpdateHorizontalLogic()
    {
        if (_rhinoStateManager.GetIsFacingRight())
            _rhinoStateManager.GetRigidBody2D().velocity = new Vector2(_rhinoStateManager.GetRunSpeed(), _rhinoStateManager.GetRigidBody2D().velocity.y);
        else
            _rhinoStateManager.GetRigidBody2D().velocity = new Vector2(-1 * _rhinoStateManager.GetRunSpeed(), _rhinoStateManager.GetRigidBody2D().velocity.y);
    }

    public override void FixedUpdate()
    {
        UpdateHorizontalLogic();
    }
}
