using UnityEngine;

public class GeckoGotHitState : GeckoBaseState
{
    public override void EnterState(GeckoStateManager geckoStateManager)
    {
        base.EnterState(geckoStateManager);
        geckoStateManager.GetAnimator().SetInteger("state", (int)EnumState.EGeckoState.gotHit);
        //Debug.Log("GH");
    }

    public override void ExitState() { }

    public override void Update()
    {

    }

    public override void FixedUpdate() { }
}
