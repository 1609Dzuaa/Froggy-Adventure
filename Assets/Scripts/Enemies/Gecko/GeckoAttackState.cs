using UnityEngine;

public class GeckoAttackState : GeckoBaseState
{
    public override void EnterState(GeckoStateManager geckoStateManager)
    {
        base.EnterState(geckoStateManager);
        geckoStateManager.GetAnimator().SetInteger("state", (int)EnumState.EGeckoState.attack);
        //Debug.Log("Attack");
    }

    public override void ExitState() { }

    public override void Update()
    {

    }

    public override void FixedUpdate() { }
}
