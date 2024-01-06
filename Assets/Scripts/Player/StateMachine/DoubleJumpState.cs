using UnityEngine;

public class DoubleJumpState : PlayerBaseState
{
    //Chỉ có thể double jump khi đã và đang jump || fall
    public override void EnterState(PlayerStateManager playerStateManager)
    {
        base.EnterState(playerStateManager);
        _playerStateManager.GetAnimator().SetInteger("state", (int)GameEnums.EPlayerState.doubleJump);
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
            if (!BuffsManager.Instance.GetTypeOfBuff(GameEnums.EBuffs.Speed).IsAllowToUpdate)
                _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetPlayerStats.SpeedX * _playerStateManager.GetDirX(), _playerStateManager.GetRigidBody2D().velocity.y);
            else
                _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetPlayerStats.SpeedX * ((PlayerSpeedBuff)BuffsManager.Instance.GetTypeOfBuff(GameEnums.EBuffs.Speed)).SpeedMultiplier * _playerStateManager.GetDirX(), _playerStateManager.GetRigidBody2D().velocity.y);
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
        return _playerStateManager.GetRigidBody2D().velocity.y < -0.1f;
    }

    private bool CheckIfCanWallSlide()
    {
        return _playerStateManager.GetIsWallTouch() && !_playerStateManager.GetIsOnGround();
    }

    private bool CheckIfCanDash()
    {
        //Debug.Log("Dashed?: " + _playerStateManager.dashState.IsFirstTimeDash);
        return Input.GetKeyDown(KeyCode.E)
             && Time.time - _playerStateManager.dashState.DashDelayStart >= _playerStateManager.GetPlayerStats.DelayDashTime
             || Input.GetKeyDown(KeyCode.E) && _playerStateManager.dashState.IsFirstTimeDash;
    }

    public override void FixedUpdate()
    {
        PhysicsUpdate();
    }

    private void HandleDbJump()
    {
        _playerStateManager.SetCanDbJump(false);
        if (!PlayerJumpBuff.Instance.IsAllowToUpdate)
            _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetRigidBody2D().velocity.x, _playerStateManager.GetPlayerStats.SpeedY * _playerStateManager.GetPlayerStats.DbJumpSpeedFactor);
        else
            _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetRigidBody2D().velocity.x, _playerStateManager.GetPlayerStats.SpeedY * PlayerJumpBuff.Instance.JumpMutiplier * _playerStateManager.GetPlayerStats.DbJumpSpeedFactor);
    }
}
