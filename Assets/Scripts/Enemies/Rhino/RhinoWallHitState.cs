using UnityEngine;

public class RhinoWallHitState : BaseState
{
    public override void EnterState(BaseStateManager _baseStateManager)
    {
        if (_baseStateManager is RhinoStateManager)
        {
            rhinoStateManager.GetAnimator().SetInteger("state", (int)EnumState.ERhinoState.wallHit);
            //Debug.Log("WH"); 
        }
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        UpdateHorizontalLogic();
        UpdateVerticalLogic();
    }

    void UpdateHorizontalLogic()
    {

    }

    void UpdateVerticalLogic()
    {

    }

    public override void FixedUpdate()
    {

    }
}
