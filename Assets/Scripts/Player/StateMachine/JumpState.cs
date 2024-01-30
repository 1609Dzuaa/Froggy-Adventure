﻿using UnityEngine;

public class JumpState : PlayerBaseState
{
    //Considering Coyote Time
    //Nếu state trước là Run có hitwall thì 0 cho WS
    //Còn nếu state trước là Run nhưng 0 hitwall thì cho phép
    private bool _isRunStateHitWall;

    public bool IsRunStateHitWall { set { _isRunStateHitWall = value; } }

    public override void EnterState(PlayerStateManager playerStateManager)
    {
        base.EnterState(playerStateManager);
        _playerStateManager.GetAnimator().SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EPlayerState.jump);
        _playerStateManager.GetDustPS().Play();
        HandleJump();
        if (_playerStateManager.GetPrevStateIsWallSlide())
            _playerStateManager.FlipSpriteAfterWallSlide();
        //Debug.Log("Jump");
    }

    public override void ExitState() { _isRunStateHitWall = false; }

    public override void Update()
    {
        //Đè dirX + jump trong lúc dính wall sẽ switch sang WallJump
        //(tương đồng với cơ chế Jump + WallJump trong Hollow Knight)
        if (CheckIfCanDbJump())
            _playerStateManager.ChangeState(_playerStateManager.doubleJumpState);
        else if (CheckIfCanFall())
            _playerStateManager.ChangeState(_playerStateManager.fallState);
        else if (CheckIfCanWallSlide())
            _playerStateManager.ChangeState(_playerStateManager.wallSlideState);
        else if (CheckIfCanWallJump())
            _playerStateManager.ChangeState(_playerStateManager.wallJumpState);
        else if (CheckIfCanDash())
            _playerStateManager.ChangeState(_playerStateManager.dashState);
    }

    private bool CheckIfCanDbJump()
    {
        return Input.GetButtonDown(GameConstants.JUMP_BUTTON) && !_playerStateManager.GetIsWallTouch();
    }

    private bool CheckIfCanFall()
    {
        return _playerStateManager.GetRigidBody2D().velocity.y < -GameConstants.NEAR_ZERO_THRESHOLD;
    }

    private bool CheckIfCanWallSlide()
    {
        return _playerStateManager.GetIsWallTouch()
            && _playerStateManager.GetDirX() * _playerStateManager.WallHit.normal.x < 0f
            && !_isRunStateHitWall;
    }

    private bool CheckIfCanWallJump()
    {
        //Đè dirX (run) va vào tường + nhấn S lúc đang Jump (current State) thì switch sang WallJump
        return _playerStateManager.GetIsWallTouch() && Input.GetButtonDown(GameConstants.JUMP_BUTTON) && _isRunStateHitWall;
    }

    private bool CheckIfCanDash()
    {
        //Debug.Log("Dashed?: " + _playerStateManager.dashState.IsFirstTimeDash);
        return Input.GetButtonDown(GameConstants.DASH_BUTTON)
             && Time.time - _playerStateManager.dashState.DashDelayStart >= _playerStateManager.GetPlayerStats.DelayDashTime
             || Input.GetButtonDown(GameConstants.DASH_BUTTON) && _playerStateManager.dashState.IsFirstTimeDash;
    }

    private void HandleJump()
    {
        _playerStateManager.JumpStart = Time.time;

        if (!BuffsManager.Instance.GetTypeOfBuff(GameEnums.EBuffs.Jump).IsAllowToUpdate)
            _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetRigidBody2D().velocity.x, _playerStateManager.GetPlayerStats.SpeedY);
        else
            _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetRigidBody2D().velocity.x, _playerStateManager.GetPlayerStats.SpeedY * ((PlayerJumpBuff)BuffsManager.Instance.GetTypeOfBuff(GameEnums.EBuffs.Jump)).JumpMutiplier);

        SoundsManager.Instance.PlaySfx(GameEnums.ESoundName.PlayerJumpSfx, 1.0f);
    }

    public override void FixedUpdate()
    {
        if (!_isRunStateHitWall)
        {
            PhysicsUpdateVertical();
            PhysicsUpdateHorizontal();
        }
    }

    private void PhysicsUpdateVertical()
    {
        if (Input.GetButton(GameConstants.JUMP_BUTTON) && Time.time - _playerStateManager.JumpStart < _playerStateManager.GetPlayerStats.JumpTime)
        {
            _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetRigidBody2D().velocity.x, _playerStateManager.GetRigidBody2D().velocity.y * _playerStateManager.GetPlayerStats.JumpSpeedFactor);
            //Debug.Log("hereeee");
        }
        //Hold càng lâu nhảy càng cao (miễn là trong thgian cho phép)
    }

    private void PhysicsUpdateHorizontal()
    {
        if (_playerStateManager.GetDirX() != 0)
            if (!BuffsManager.Instance.GetTypeOfBuff(GameEnums.EBuffs.Speed).IsAllowToUpdate)
                _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetPlayerStats.SpeedX * _playerStateManager.GetDirX(), _playerStateManager.GetRigidBody2D().velocity.y);
            else
                _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetPlayerStats.SpeedX * ((PlayerSpeedBuff)BuffsManager.Instance.GetTypeOfBuff(GameEnums.EBuffs.Speed)).SpeedMultiplier * _playerStateManager.GetDirX(), _playerStateManager.GetRigidBody2D().velocity.y);
    }

}
