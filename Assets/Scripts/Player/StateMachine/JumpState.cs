using UnityEngine;
using static GameEnums;
using static GameConstants;

public class JumpState : PlayerBaseState
{
    //Considering Coyote Time
    //Nếu state trước là Run có hitwall thì 0 cho WS
    //Còn nếu state trước là Run nhưng 0 hitwall thì cho phép
    private bool _isRunStateHitWall;
    private float _jumpForceApplied = 0f;

    public float JumpForceApplied { get => _jumpForceApplied; set => _jumpForceApplied = value; }

    public bool IsRunStateHitWall { set { _isRunStateHitWall = value; } }

    public override void EnterState(PlayerStateManager playerStateManager)
    {
        base.EnterState(playerStateManager);
        _playerStateManager.GetAnimator().SetInteger(ANIM_PARA_STATE, (int)EPlayerState.jump);
        _playerStateManager.GetDustPS().Play();
        HandleJump();
        if (_playerStateManager.GetPrevStateIsWallSlide())
            _playerStateManager.FlipSpriteAfterWallSlide();
        //Debug.Log("Jump");
    }

    public override void ExitState() { _isRunStateHitWall = false; _jumpForceApplied = 0; }

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
        //Debug.Log("can: " + _playerStateManager.JumpDetect.DbJump);
        return _playerStateManager.BtnJumpControl.DbJump
            && !_playerStateManager.GetIsWallTouch()
            && Time.time - _playerStateManager.JumpStart >= DELAY_PLAYER_DOUBLE_JUMP;
    }

    private bool CheckIfCanFall()
    {
        return _playerStateManager.GetRigidBody2D().velocity.y < -NEAR_ZERO_THRESHOLD;
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
        return _playerStateManager.GetIsWallTouch() && _playerStateManager.BtnJumpControl.DbJump
            /*Input.GetButtonDown(JUMP_BUTTON)*/ && _isRunStateHitWall;
    }

    private bool CheckIfCanDash()
    {
        //Debug.Log("Dashed?: " + _playerStateManager.dashState.IsFirstTimeDash);
        return _playerStateManager.BtnDashControl.IsDashing
             && Time.time - _playerStateManager.dashState.DashDelayStart >= _playerStateManager.GetPlayerStats.DelayDashTime
             || Input.GetButtonDown(DASH_BUTTON) && _playerStateManager.dashState.IsFirstTimeDash;
    }

    private void HandleJump()
    {
        _playerStateManager.JumpStart = Time.time;

        if (_jumpForceApplied != 0)
        {
            float xVelo = _playerStateManager.GetRigidBody2D().velocity.x;
            float yVelo = _jumpForceApplied;
            _playerStateManager.GetRigidBody2D().velocity = new(xVelo, yVelo);
        }
        else
        {
            float xVelo = _playerStateManager.GetRigidBody2D().velocity.x;
            float yVelo = _playerStateManager.JumpSpeed;
            _playerStateManager.GetRigidBody2D().velocity = new(xVelo, yVelo);
        }

        SoundsManager.Instance.PlaySfx(ESoundName.PlayerJumpSfx, 1.0f);
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
        if (_playerStateManager.BtnJumpControl.IsHolding && Time.time - _playerStateManager.JumpStart < _playerStateManager.GetPlayerStats.JumpTime)
        {
            _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetRigidBody2D().velocity.x, _playerStateManager.GetRigidBody2D().velocity.y * _playerStateManager.GetPlayerStats.JumpSpeedFactor);
            //Debug.Log("hereeee");
        }
        //Hold càng lâu nhảy càng cao (miễn là trong thgian cho phép)
    }

    private void PhysicsUpdateHorizontal()
    {
        if (_playerStateManager.GetDirX() != 0)
            _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.MoveSpeed * _playerStateManager.GetDirX(), _playerStateManager.GetRigidBody2D().velocity.y);
    }

}
