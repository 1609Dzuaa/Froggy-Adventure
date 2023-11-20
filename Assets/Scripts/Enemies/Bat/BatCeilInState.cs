using UnityEngine;

public class BatCeilInState : BatBaseState
{
    public override void EnterState(BatStateManager batStateManager)
    {
        base.EnterState(batStateManager);
        _batStateManager.GetAnimator().SetInteger("state", (int)EnumState.EBatState.ceilIn);
        Debug.Log("Ceil In");
    }

    public override void ExitState() { }

    public override void UpdateState() { }

    public override void FixedUpdate() { }
}
