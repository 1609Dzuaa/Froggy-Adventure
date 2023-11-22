using UnityEngine;

public class GeckoHideState : GeckoBaseState
{
    public override void EnterState(GeckoStateManager geckoStateManager)
    {
        base.EnterState(geckoStateManager);
        geckoStateManager.GetAnimator().SetInteger("state", (int)EnumState.EGeckoState.hide);
        //Debug.Log("Hide");
    }

    public override void ExitState() { }

    public override void Update()
    {

    }

    public override void FixedUpdate() { }
}
