using UnityEngine;

public class BatCeilOutState : BatBaseState
{
    public override void EnterState(BatStateManager batStateManager)
    {
        base.EnterState(batStateManager);
        _batStateManager.GetAnimator().SetInteger("state", (int)EnumState.EBatState.ceilOut);
        Debug.Log("CO");
    }

    public override void ExitState() { }

    public override void UpdateState() 
    { 

    }

    public override void FixedUpdate() { }
}
