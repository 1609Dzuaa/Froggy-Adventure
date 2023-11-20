using UnityEngine;

public class BatIdleState : BatBaseState
{
    private bool allowUpdate = false;

    public void SetTrueAllowUpdate() { this.allowUpdate = true; }

    public override void EnterState(BatStateManager batStateManager)
    {
        base.EnterState(batStateManager);
        _batStateManager.GetAnimator().SetInteger("state", (int)EnumState.EBatState.idle);
        Debug.Log("Idle");
    }

    public override void ExitState() 
    {
        allowUpdate = false;
    }

    public override void UpdateState() 
    {
        if(allowUpdate)
        {
            float distance;
            distance = Vector2.Distance(_batStateManager.transform.position, _batStateManager.GetPlayer().position);
            if (distance > 0)
            {
                _batStateManager.ChangState(_batStateManager.batChaseState);
            }
        }
    }

    public override void FixedUpdate() { }
}
