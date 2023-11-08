using Unity.VisualScripting;
using UnityEngine;

public class GotHitState : BaseState
{
    private bool allowUpdate = false;

    public void SetAllowUpdate(bool para) { this.allowUpdate = para; }

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
        //Chỉ cho phép Update sau khi chạy xong animation Hit
        if(allowUpdate)
        {
            if (playerStateManager.GetDirX() == 0)
                playerStateManager.ChangeState(playerStateManager.idleState);
            else if (playerStateManager.GetDirX() != 0)
                playerStateManager.ChangeState(playerStateManager.runState);
            else if (Input.GetKeyDown(KeyCode.S))
                playerStateManager.ChangeState(playerStateManager.jumpState);
        }
        //Debug.Log("Allow: " + allowUpdate);
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
        allowUpdate = false;
        playerStateManager.DecreaseHP();
        playerStateManager.GetGotHitSound().Play();
        playerStateManager.Invoke("ChangeToIdle", 0.48f);
        //Why 0.35f ?
        //Vì Animation GotHit có tổng thgian là 0.467s
        //=> Gọi hàm change Idle sau .35s
    }
}
