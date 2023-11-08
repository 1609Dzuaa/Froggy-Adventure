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
        HandleGotHit();
        //Chú ý khi làm việc với Any State
        //Tắt Transition To Self ở đoạn nối Transition từ Any State tới State cụ thể
        //Tránh bị đứng ngay frame đầu tiên
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
        {
            playerStateManager.GetRigidBody2D().AddForce(new Vector2(-1 * playerStateManager.GetKnockBackForce(), playerStateManager.GetRigidBody2D().velocity.y));
        }
        else
        {
            playerStateManager.GetRigidBody2D().AddForce(new Vector2(playerStateManager.GetKnockBackForce(), playerStateManager.GetRigidBody2D().velocity.y));
        }
    }

    private void HandleGotHit()
    {
        KnockBack();
        playerStateManager.DecreaseHP();
        playerStateManager.GetGotHitSound().Play();
        playerStateManager.Invoke("ChangeToIdle", 0.35f);
        //Why 0.35f ?
        //Vì Animation GotHit có tổng thgian là 0.318s
        //=> Gọi hàm change Idle sau .35s
    }
}
