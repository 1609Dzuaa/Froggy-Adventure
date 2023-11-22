using UnityEngine;

public class BatRetreatState : BatBaseState
{
    public override void EnterState(BatStateManager batStateManager)
    {
        base.EnterState(batStateManager);
        _batStateManager.GetAnimator().SetInteger("state", (int)EnumState.EBatState.retreat);
        Debug.Log("Rt");
    }

    public override void ExitState() { }

    public override void Update() 
    {
        _batStateManager.transform.position = Vector2.MoveTowards(_batStateManager.transform.position, _batStateManager.GetSleepPos().position, _batStateManager.GetChaseSpeed() * Time.deltaTime);
        if (Vector2.Distance(_batStateManager.transform.position,_batStateManager.GetSleepPos().position) < 0.1f)
            _batStateManager.ChangState(_batStateManager.batCeilInState);
    }

    public override void FixedUpdate() { }
}
