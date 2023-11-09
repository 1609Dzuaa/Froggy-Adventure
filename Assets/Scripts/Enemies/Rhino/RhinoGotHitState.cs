using UnityEngine;

public class RhinoGotHitState : BaseState
{
    public override void EnterState(BaseStateManager _baseStateManager)
    {
        if (_baseStateManager is RhinoStateManager)
        {
            rhinoStateManager.GetAnimator().SetInteger("state", (int)EnumState.ERhinoState.gotHit);
            rhinoStateManager.GetBoxCollider2D().enabled = false;
            Debug.Log("GotHit"); //Keep this, use for debugging change state
        }
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
       
    }

    public override void FixedUpdate()
    {

    }
}
