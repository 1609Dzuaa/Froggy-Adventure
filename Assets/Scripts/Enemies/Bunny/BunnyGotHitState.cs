using UnityEngine;

public class BunnyGotHitState : BunnyBaseState
{
    public override void EnterState(BunnyStateManager bunnyStateManager)
    {
        base.EnterState(bunnyStateManager);
        _bunnyStateManager.GetAnimator().SetInteger("state", (int)EnumState.EBunnyState.gotHit);

    }

    public override void ExitState() { }

    public override void Update() { }

    public override void FixedUpdate() { }

}
