using UnityEngine;

public class BatGotHitState : BatBaseState
{
    public override void EnterState(BatStateManager batStateManager)
    {
        base.EnterState(batStateManager);
        _batStateManager.GetAnimator().SetInteger("state", (int)EnumState.EBatState.gotHit);
    }

    public override void ExitState() { }

    public override void UpdateState() { }

    public override void FixedUpdate() { }
}
