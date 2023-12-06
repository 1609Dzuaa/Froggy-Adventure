using UnityEngine;

public class WallJumpState : PlayerBaseState
{
    //2 biến để dùng đếm giờ và đánh dấu hết bị disable input directionX
    private float _disableStart;
    private bool _isEndDisable;

    public bool IsEndDisable { get { return _isEndDisable; } }

    public override void EnterState(PlayerStateManager playerStateManager)
    {
        base.EnterState(playerStateManager);
        _playerStateManager.GetAnimator().SetInteger("state", (int)EnumState.EPlayerState.wallJump);
        _playerStateManager.FlipSpriteAfterWallSlide();
        HandleWallJump();
        Debug.Log("WallJump");
        _disableStart = Time.time;
    }

    public override void ExitState()
    {
        _isEndDisable = false;
    }

    public override void Update()
    {
        if (CheckIfCanDbJump())
            _playerStateManager.ChangeState(_playerStateManager.doubleJumpState);
        else if (CheckIfCanFall())
            _playerStateManager.ChangeState(_playerStateManager.fallState);
        else if (CheckIfCanWallSlide())
            _playerStateManager.ChangeState(_playerStateManager.wallSlideState);
    }

    private bool CheckIfCanDbJump()
    {
        //Press S While WallJump => Double Jump
        if (Input.GetKeyDown(KeyCode.S))
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
        if (_playerStateManager.GetIsWallTouch() && _isEndDisable)
        {
            //Debug.Log("ws here"); 
            return true;
        }
        return false;
    }

    public override void FixedUpdate()
    {
        if(Time.time - _disableStart >= _playerStateManager.DisableTime)
        {
            _isEndDisable = true;
            if (_playerStateManager.GetDirX() != 0)
                _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetSpeedX() * _playerStateManager.GetDirX(), _playerStateManager.GetRigidBody2D().velocity.y);
        }
    }

    private void HandleWallJump()
    {
        //Nếu nhảy khi đang trượt tường thì nhảy xéo
        if (_playerStateManager.WallHit.normal.x == 1f)
            _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetWallJumpSpeedX(), _playerStateManager.GetWallJumpSpeedY());
        else if (_playerStateManager.WallHit.normal.x == -1f)
            _playerStateManager.GetRigidBody2D().velocity = new Vector2(-_playerStateManager.GetWallJumpSpeedX(), _playerStateManager.GetWallJumpSpeedY());
    }
}
