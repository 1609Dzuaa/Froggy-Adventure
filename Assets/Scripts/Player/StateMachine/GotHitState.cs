using Unity.VisualScripting;
using UnityEngine;

public class GotHitState : PlayerBaseState
{
    private bool allowUpdate = false;

    public void SetAllowUpdate(bool para) { this.allowUpdate = para; }

    public override void EnterState(PlayerStateManager playerStateManager)
    {
        base.EnterState(playerStateManager);
        _playerStateManager.GetAnimator().SetInteger("state", (int)EnumState.EPlayerState.gotHit);
        //Debug.Log("GotHit");
        HandleGotHit();
        //Chú ý khi làm việc với Any State
        //Tắt Transition To Self ở đoạn nối Transition từ Any State tới State cụ thể
        //Tránh bị đứng ngay frame đầu tiên
    }

    public override void ExitState()
    {

    }

    public override void Update()
    {
        //Chỉ cho phép Update sau khi chạy xong animation Hit
        if(allowUpdate)
        {
            if (_playerStateManager.GetDirX() == 0)
                _playerStateManager.ChangeState(_playerStateManager.idleState);
            else if (_playerStateManager.GetDirX() != 0)
                _playerStateManager.ChangeState(_playerStateManager.runState);
            else if (Input.GetKeyDown(KeyCode.S))
                _playerStateManager.ChangeState(_playerStateManager.jumpState);
        }
        //Debug.Log("Allow: " + allowUpdate);
    }

    public override void FixedUpdate()
    {

    }

    private void KnockBack()
    {
        if (_playerStateManager.GetIsFacingRight())
        {
            _playerStateManager.GetRigidBody2D().AddForce(new Vector2(-1 * _playerStateManager.GetKnockBackForce(), 0f));
        }
        else
        {
            _playerStateManager.GetRigidBody2D().AddForce(new Vector2(_playerStateManager.GetKnockBackForce(), 0f));
        }
        //Debug.Log("Knock");
        //Chỉnh lại Func này: hướng addForce
    }

    private void HandleGotHit()
    {
        allowUpdate = false;
        _playerStateManager.DecreaseHP();
        _playerStateManager.GetGotHitSound().Play();
        _playerStateManager.Invoke("ChangeToIdle", 0.48f);
        //Why 0.48f ?
        //Vì Animation GotHit có tổng thgian là 0.467s
        //=> Gọi hàm change Idle sau .48s
    }
}
