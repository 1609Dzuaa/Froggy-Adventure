using UnityEngine;

public class RhinoRunState : BaseState
{
    public override void EnterState(BaseStateManager _baseStateManager)
    {
        if (_baseStateManager is RhinoStateManager)
        {
            rhinoStateManager.GetAnimator().SetInteger("state", (int)EnumState.ERhinoState.run);
            Debug.Log("Run");
        }
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        
    }

    void UpdateHorizontalLogic()
    {
        if (rhinoStateManager.GetIsFacingRight())
            rhinoStateManager.GetRigidBody2D().velocity = new Vector2(rhinoStateManager.GetRunSpeed(), rhinoStateManager.GetRigidBody2D().velocity.y);
        else
            rhinoStateManager.GetRigidBody2D().velocity = new Vector2(-1 * rhinoStateManager.GetRunSpeed(), rhinoStateManager.GetRigidBody2D().velocity.y);
    }

    public override void FixedUpdate()
    {
        UpdateHorizontalLogic();
    }
}
