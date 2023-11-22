using UnityEngine;

public class GeckoIdleState : GeckoBaseState
{
    private float entryTime;

    public override void EnterState(GeckoStateManager geckoStateManager)
    {
        base.EnterState(geckoStateManager);
        _geckoStateManager.GetAnimator().SetInteger("state", (int)EnumState.EGeckoState.idle);
        entryTime = Time.time;
        //Debug.Log("Idle");
    }

    public override void ExitState() { }

    public override void Update()
    {
        if (Time.time - entryTime >= _geckoStateManager.GetRestTime())
            _geckoStateManager.ChangeState(_geckoStateManager.geckoPatrolState);
        else if (_geckoStateManager.GetHasDetectPlayer())
            _geckoStateManager.ChangeState(_geckoStateManager.geckoHideState);
    }

    public override void FixedUpdate() { }
}
