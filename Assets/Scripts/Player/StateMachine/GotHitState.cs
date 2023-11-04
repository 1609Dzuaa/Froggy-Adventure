using Unity.VisualScripting;
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
        KnockBack();
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

    private void KnockBack()
    {
        if (playerStateManager.GetIsFacingRight())
            playerStateManager.GetRigidBody2D().AddForce(new Vector2(-1 * playerStateManager.GetKnockBackSpeed(), playerStateManager.GetRigidBody2D().velocity.y));
        else
            playerStateManager.GetRigidBody2D().AddForce(new Vector2(playerStateManager.GetKnockBackSpeed(), playerStateManager.GetRigidBody2D().velocity.y));
    }
}
