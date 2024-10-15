using UnityEngine;

public class DoubleJumpState : PlayerBaseState
{
    //Chỉ có thể double jump khi đã và đang jump || fall
    public override void EnterState(PlayerStateManager playerStateManager)
    {
        base.EnterState(playerStateManager);
        _playerStateManager.GetAnimator().SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EPlayerState.doubleJump);
        HandleDbJump();
        //Debug.Log("DBJump");
    }

    public override void ExitState() { }

    public override void Update()
    {
        LogicUpdate();
    }

    private void PhysicsUpdate()
    {
        if (_playerStateManager.GetDirX() != 0)
            if (!BuffsManager.Instance.GetTypeOfBuff(GameEnums.EBuffs.Speed).IsActivating)
                _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.MoveSpeed * _playerStateManager.GetDirX(), _playerStateManager.GetRigidBody2D().velocity.y);
            else
                _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.MoveSpeed * ((PlayerSpeedBuff)BuffsManager.Instance.GetTypeOfBuff(GameEnums.EBuffs.Speed)).SpeedMultiplier * _playerStateManager.GetDirX(), _playerStateManager.GetRigidBody2D().velocity.y);
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
        return _playerStateManager.GetIsWallTouch() && !_playerStateManager.GetIsOnGround();
    }

    private bool CheckIfCanDash()
    {
        //Debug.Log("Dashed?: " + _playerStateManager.dashState.IsFirstTimeDash);
        return Input.GetButtonDown(GameConstants.DASH_BUTTON)
             && Time.time - _playerStateManager.dashState.DashDelayStart >= _playerStateManager.GetPlayerStats.DelayDashTime
             || Input.GetButtonDown(GameConstants.DASH_BUTTON) && _playerStateManager.dashState.IsFirstTimeDash;
    }

    public override void FixedUpdate()
    {
        PhysicsUpdate();
    }

    private void HandleDbJump()
    {
        _playerStateManager.SetCanDbJump(false);
        if (!BuffsManager.Instance.GetTypeOfBuff(GameEnums.EBuffs.Jump).IsActivating)
            _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetRigidBody2D().velocity.x, _playerStateManager.GetPlayerStats.SpeedY * _playerStateManager.GetPlayerStats.DbJumpSpeedFactor);
        else
            _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetRigidBody2D().velocity.x, _playerStateManager.GetPlayerStats.SpeedY * ((PlayerJumpBuff)BuffsManager.Instance.GetTypeOfBuff(GameEnums.EBuffs.Jump)).JumpMutiplier * _playerStateManager.GetPlayerStats.DbJumpSpeedFactor);
        SoundsManager.Instance.PlaySfx(GameEnums.ESoundName.DoubleJumpSfx, 1.0f);
    }
}
