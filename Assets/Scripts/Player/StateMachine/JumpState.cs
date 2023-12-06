using UnityEngine;

public class JumpState : PlayerBaseState
{
    //Considering Coyote Time
    private float _xStart;
    private float _nWall;
    private float _disableStart;

    public float NWall { set { _nWall = value; } }

    public override void EnterState(PlayerStateManager playerStateManager)
    {
        base.EnterState(playerStateManager);
        _playerStateManager.GetAnimator().SetInteger("state", (int)EnumState.EPlayerState.jump);
        _playerStateManager.GetDustPS().Play();
        HandleJump();
        //Nếu state trước là WS thì tức là đang WallJump
        if (_playerStateManager.GetPrevStateIsWallSlide())
            _playerStateManager.FlipSpriteAfterWallSlide();
        _xStart = _playerStateManager.transform.position.x;
        Debug.Log("Jump: " + _nWall);
        _disableStart = Time.time;
        //cung` dau: WS, JUMP, JUMP, WS
    }

    public override void ExitState() { _nWall = 0; }

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
        //Press S While Jump => Double Jump
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
        if (_playerStateManager.GetIsWallTouch()) 
        { 
            //Debug.Log("ws here"); 
            return true; 
        }
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
