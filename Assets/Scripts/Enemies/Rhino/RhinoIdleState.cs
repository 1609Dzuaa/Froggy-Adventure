using UnityEngine;

public class RhinoIdleState : BaseState
{
    public override void EnterState(BaseStateManager _baseStateManager)
    {
        if (_baseStateManager is RhinoStateManager)
        {
            rhinoStateManager = (RhinoStateManager)_baseStateManager;
            rhinoStateManager.GetAnimator().SetInteger("state", (int)EnumState.ERhinoState.idle);
            Debug.Log("Idle"); //Keep this, use for debugging change state
        }
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        if(playerStateManager.transform.position.x > rhinoStateManager.transform.position.x) 
        {
            //
        }
    }

    public override void FixedUpdate()
    {

    }
}
