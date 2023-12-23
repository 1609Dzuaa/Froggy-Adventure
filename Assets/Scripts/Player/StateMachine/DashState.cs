using UnityEngine;

public class DashState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager playerStateManager)
    {
        base.EnterState(playerStateManager);
        _playerStateManager.GetAnimator().SetInteger("state", (int)EnumState.EPlayerState.dash);
        _playerStateManager.GetDustPS().Play();
        HandleDash();
        if (_playerStateManager.GetPrevStateIsWallSlide())
            _playerStateManager.FlipSpriteAfterWallSlide();
        //Debug.Log("Jump");
    }

    public override void ExitState() { }

    public override void Update()
    {
        /*if (CheckIfCanDbJump())
            _playerStateManager.ChangeState(_playerStateManager.doubleJumpState);
        else*/ 
        if (CheckIfCanFall())
            _playerStateManager.ChangeState(_playerStateManager.fallState);
        else if (CheckIfCanWallSlide())
            _playerStateManager.ChangeState(_playerStateManager.wallSlideState);
        /*else if (CheckIfCanWallJump())
            _playerStateManager.ChangeState(_playerStateManager.wallJumpState);*/
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
        return _playerStateManager.GetRigidBody2D().velocity.y < -0.1f;
    }

    private bool CheckIfCanWallSlide()
    {
        return _playerStateManager.GetIsWallTouch()
            && _playerStateManager.GetDirX() * _playerStateManager.WallHit.normal.x < 0f;
    }

    /*private bool CheckIfCanWallJump()
    {
        //Đè dirX (run) va vào tường + nhấn S lúc đang Jump (current State) thì switch sang WallJump
        if (_playerStateManager.GetIsWallTouch() && Input.GetKeyDown(KeyCode.S) && _isRunStateHitWall)
            return true;
        return false;
    }*/

    private void HandleDash()
    {
        if (_playerStateManager.GetIsFacingRight())
            _playerStateManager.GetRigidBody2D().AddForce(_playerStateManager.GetPlayerStats.DashForce, ForceMode2D.Impulse);
        else
            _playerStateManager.GetRigidBody2D().AddForce(_playerStateManager.GetPlayerStats.DashForce * new Vector2(-1f, 1f), ForceMode2D.Impulse);

        _playerStateManager.GetJumpSound().Play();
    }

    public override void FixedUpdate()
    {
        //nothing
    }

}
