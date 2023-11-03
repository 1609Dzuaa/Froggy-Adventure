using UnityEngine;

public class GotHitState : BaseState
{
    public override void EnterState(BaseStateManager _baseStateManager)
    {
        if (_baseStateManager is PlayerStateManager)
        {
            playerStateManager.GetAnimator().SetInteger("state", (int)EnumState.EState.gotHit);
            //Debug.Log("GotHit");
        }
        playerStateManager.DecreaseHP();
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        
    }

    public override void FixedUpdate()
    {

    }
}
