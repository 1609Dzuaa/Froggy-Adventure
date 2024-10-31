using UnityEngine;

public class DoubleJumpState : PlayerBaseState
{
    //Chỉ có thể double jump khi đã và đang jump || fall
    public override void EnterState(PlayerStateManager playerStateManager)
    {
        base.EnterState(playerStateManager);
        _playerStateManager.GetAnimator().SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EPlayerState.doubleJump);
        HandleDbJump();
        Debug.Log("DBJump");
    }

    public override void ExitState() { }

    public override void Update()
    {
        LogicUpdate();
    }

    private void PhysicsUpdate()
    {
        if (_playerStateManager.GetDirX() != 0)
            _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.MoveSpeed * _playerStateManager.GetDirX(), _playerStateManager.GetRigidBody2D().velocity.y);
    }

    private void LogicUpdate()
    {
        if (CheckIfCanFall())
            _playerStateManager.ChangeState(_playerStateManager.fallState);
        else if (CheckIfCanWallSlide())
            _playerStateManager.ChangeState(_playerStateManager.wallSlideState);
        else if (CheckIfCanDash())
            _playerStateManager.ChangeState(_playerStateManager.dashState);
    }

    private bool CheckIfCanFall()
    {
        return _playerStateManager.GetRigidBody2D().velocity.y < -GameConstants.NEAR_ZERO_THRESHOLD;
    }

    private bool CheckIfCanWallSlide()
    {
        return _playerStateManager.GetIsWallTouch()
            && _playerStateManager.UnlockedWallSlide
            && !_playerStateManager.GetIsOnGround();
    }

    private bool CheckIfCanDash()
    {
        //Debug.Log("Dashed?: " + _playerStateManager.dashState.IsFirstTimeDash);
        return _playerStateManager.BtnDashControl.IsDashing
             && Time.time - _playerStateManager.dashState.DashDelayStart >= _playerStateManager.GetPlayerStats.DelayDashTime
             || _playerStateManager.BtnDashControl.IsDashing && _playerStateManager.dashState.IsFirstTimeDash;
    }

    public override void FixedUpdate()
    {
        PhysicsUpdate();
    }

    private void HandleDbJump()
    {
        _playerStateManager.SetCanDbJump(false);
        _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetRigidBody2D().velocity.x, _playerStateManager.JumpSpeed);
        SoundsManager.Instance.PlaySfx(GameEnums.ESoundName.DoubleJumpSfx, 1.0f);
    }
}
