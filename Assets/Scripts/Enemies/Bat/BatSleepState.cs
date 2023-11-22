using UnityEngine;

public class BatSleepState : BatBaseState
{
    private float entryTime;
    public override void EnterState(BatStateManager batStateManager) 
    {
        base.EnterState(batStateManager);
        _batStateManager.GetAnimator().SetInteger("state", (int)EnumState.EBatState.sleep);
        entryTime = Time.time;
        Debug.Log("Sleep");
    }

    public override void ExitState() { }

    public override void Update() 
    {
        if (Time.time - entryTime >= _batStateManager.GetSleepTime())
            _batStateManager.ChangState(_batStateManager.batCeilOutState);
    }

    public override void FixedUpdate() { }
}
