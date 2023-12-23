using UnityEngine;

public class DashState : PlayerBaseState
{
    private bool _allowUpdate;
    private bool _isEndDisable;
    private float _dashDelayStart; //Đếm giờ sau khi vừa Dash xong, để sau _delayDashTime (s) thì mới đc Dash tiếp
    private float _disableStart; //If previous state is WS
    private bool _isFirstTimeDash = true; //thêm th này
    //vì có thể có TH vào scene cái là nhấn E để dash luôn
    //nhưng 0 đc và phải đợi _delayDashTime (s)

    public bool AllowUpdate { set {  _allowUpdate = value; } }

    public bool IsEndDisable { get { return _isEndDisable; } }

    public float DashDelayStart { get { return _dashDelayStart; } }

    public bool IsFirstTimeDash { get { return _isFirstTimeDash; } }

    public override void EnterState(PlayerStateManager playerStateManager)
    {
        base.EnterState(playerStateManager);
        _playerStateManager.GetAnimator().SetInteger("state", (int)EnumState.EPlayerState.dash);
        _playerStateManager.GetDustPS().Play();
        _isFirstTimeDash = false;
        HandleIfPrevStateWallSlide();
        HandleDash();
        //Debug.Log("Jump");
    }

    public override void ExitState() 
    { 
        _allowUpdate = false;
        _dashDelayStart = Time.time;
        _playerStateManager.GetTrailRenderer().emitting = false;
    }

    public override void Update()
    {
        if (_allowUpdate)
        {
            //Trả grav về lại như cũ sau khi cho phép Update
            _playerStateManager.GetRigidBody2D().gravityScale = _playerStateManager.GetPlayerStats.GravScale;

            if (CheckIfCanDbJump())
                _playerStateManager.ChangeState(_playerStateManager.doubleJumpState);
            else if (CheckIfCanFall())
                _playerStateManager.ChangeState(_playerStateManager.fallState);
            else if (CheckIfCanWallSlide())
            {
                //Debug.Log("here");
                _playerStateManager.ChangeState(_playerStateManager.wallSlideState);
            }
            else if (CheckIfCanIdle())
                _playerStateManager.ChangeState(_playerStateManager.idleState);
            else if (!CheckIfCanIdle())
                _playerStateManager.ChangeState(_playerStateManager.runState);

            if (Time.time - _disableStart >= _playerStateManager.DisableTime && _playerStateManager.GetPrevStateIsWallSlide())
                _isEndDisable = true;
        }
    }

    private bool CheckIfCanIdle()
    {
        return _playerStateManager.GetDirX() == 0;
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

    private void HandleDash()
    {
        //Vô hiệu hoá grav khi dash (cho ảo hơn)
        _playerStateManager.GetRigidBody2D().gravityScale = 0f;
        _playerStateManager.GetDashSound().Play();
        _playerStateManager.GetTrailRenderer().emitting = true; //Ch thấy effect đâu @@?

        if (_playerStateManager.GetIsFacingRight())
            _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetPlayerStats.DashForce.x, 0f);
        else
            _playerStateManager.GetRigidBody2D().velocity = new Vector2(-_playerStateManager.GetPlayerStats.DashForce.x, 0f);
    }

    private void HandleIfPrevStateWallSlide()
    {
        //State WS sẽ có đặc biệt chút là sẽ flip sprite ngược với isFacingRight
        //Ta sẽ check nếu biến bool prev State là WS thì flip ngược lại
        //và set lại biến đó sau khi flip xong 
        if (_playerStateManager.GetPrevStateIsWallSlide())
        {
            _playerStateManager.FlipSpriteAfterWallSlide();
            _disableStart = Time.time;
        }
    }

    public override void FixedUpdate()
    {
        //nothing
    }

}
