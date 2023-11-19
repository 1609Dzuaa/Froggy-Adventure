using UnityEngine;

public class DoubleJumpState : PlayerBaseState
{
    //Chỉ có thể double jump khi đã và đang jump || fall
    public override void EnterState(PlayerStateManager playerStateManager)
    {
        base.EnterState(playerStateManager);
        _playerStateManager.GetAnimator().SetInteger("state", (int)EnumState.EPlayerState.doubleJump);

        HandleDbJump();
        //Debug.Log("DBJump");
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        UpdateVerticalLogic();
    }

    private void UpdateHorizontalPhysics()
    {
        if (_playerStateManager.GetDirX() != 0)
        {
            _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetSpeedX() * _playerStateManager.GetDirX(), _playerStateManager.GetRigidBody2D().velocity.y);
        }
    }

    private void UpdateVerticalLogic()
    {
        if (_playerStateManager.GetRigidBody2D().velocity.y < -0.1f)
            _playerStateManager.ChangeState(_playerStateManager.fallState);
        else if (_playerStateManager.GetIsWallTouch() && !_playerStateManager.GetIsOnGround())
            _playerStateManager.ChangeState(_playerStateManager.wallSlideState);
    }

    public override void FixedUpdate()
    {
        UpdateHorizontalPhysics();
    }

    private void HandleDbJump()
    {
        _playerStateManager.SetHasDbJump(true);
        _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetRigidBody2D().velocity.x, _playerStateManager.GetSpeedY() * 0.9f);
    }
}
