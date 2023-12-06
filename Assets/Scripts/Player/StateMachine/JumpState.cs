using UnityEngine;

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
        _playerStateManager.GetAnimator().SetInteger("state", (int)EnumState.EPlayerState.jump);
        _playerStateManager.GetDustPS().Play();
        HandleJump();
        if (_playerStateManager.GetPrevStateIsWallSlide())
            _playerStateManager.FlipSpriteAfterWallSlide();
        Debug.Log("Jump");
    }

    public override void ExitState() { _isRunStateHitWall = false; }

    public override void Update()
    {
        //Đè dirX + S + S trong lúc Jump sẽ switch sang WallJump
        //(tương đồng với cơ chế Jump + WallJump trong Hollow Knight)
        if (CheckIfCanDbJump())
            _playerStateManager.ChangeState(_playerStateManager.doubleJumpState);
        else if (CheckIfCanFall())
            _playerStateManager.ChangeState(_playerStateManager.fallState);
        else if (CheckIfCanWallSlide())
            _playerStateManager.ChangeState(_playerStateManager.wallSlideState);
        else if (CheckIfCanWallJump())
            _playerStateManager.ChangeState(_playerStateManager.wallJumpState);
    }

    private bool CheckIfCanDbJump()
    {
        //Press S While Jump and not touching wall => Double Jump
        if (Input.GetKeyDown(KeyCode.S) && !_playerStateManager.GetIsWallTouch())
           return true;
        return false;
    }

    private bool CheckIfCanFall()
    {
        if (_playerStateManager.GetRigidBody2D().velocity.y < -0.1f)
            return true;
        return false;
    }

    private bool CheckIfCanWallSlide()
    {
        if (_playerStateManager.GetIsWallTouch()
            && _playerStateManager.GetDirX() * _playerStateManager.WallHit.normal.x < 0f
            && !_isRunStateHitWall)
            return true;
        return false;
    }

    private bool CheckIfCanWallJump()
    {
        //Đè dirX (run) va vào tường + nhấn S lúc đang Jump (current State) thì switch sang WallJump
        if (_playerStateManager.GetIsWallTouch() && Input.GetKeyDown(KeyCode.S) && _isRunStateHitWall)
            return true;
        return false;
    }

    private void HandleJump()
    {
        _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetRigidBody2D().velocity.x, _playerStateManager.GetSpeedY());
        _playerStateManager.GetJumpSound().Play();
    }

    public override void FixedUpdate()
    {
        if (_playerStateManager.GetDirX() != 0)
            _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetSpeedX() * _playerStateManager.GetDirX(), _playerStateManager.GetRigidBody2D().velocity.y);
    }

}
