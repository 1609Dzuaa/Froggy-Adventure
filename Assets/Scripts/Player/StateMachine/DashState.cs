using UnityEngine;

public class DashState : PlayerBaseState
{
    private bool _allowUpdate;
    private float _dashDelayStart; //Đếm giờ sau khi vừa Dash xong, để sau _delayDashTime (s) thì mới đc Dash tiếp
    private bool _isFirstTimeDash = true; //thêm th này
    //vì có thể có TH vào scene cái là nhấn E để dash luôn
    //nhưng 0 đc và phải đợi _delayDashTime (s)

    public bool AllowUpdate { set {  _allowUpdate = value; } }

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
        Debug.Log("Dash");
    }

    public override void ExitState() 
    { 
        _allowUpdate = false;
        _dashDelayStart = Time.time;
        _playerStateManager.GetTrailRenderer().emitting = false;
        _playerStateManager.gameObject.layer = LayerMask.NameToLayer(GameConstants.PLAYER_LAYER);
        _playerStateManager.IsDashing = false;
        //Thêm dòng dưới khi Exit phòng TH: dash nhưng ch update thì đã exit state 
        //dẫn đến grav scale = 0
        _playerStateManager.GetRigidBody2D().gravityScale = _playerStateManager.GetPlayerStats.GravScale;
    }

    public override void Update()
    {
        if (_allowUpdate)
        {
            //Trả grav về lại như cũ sau khi cho phép Update
            _playerStateManager.GetRigidBody2D().gravityScale = _playerStateManager.GetPlayerStats.GravScale;

            if (CheckIfCanIdle())
            {
                Debug.Log("OG: " + _playerStateManager.GetIsOnGround());
                _playerStateManager.ChangeState(_playerStateManager.idleState);
            }
            else if(CheckIfCanRun())
                _playerStateManager.ChangeState(_playerStateManager.runState);
            else if (CheckIfCanFall())
            {
                //Đảm bảo 0 dashing quá xa sau khi fall
                _playerStateManager.GetRigidBody2D().velocity = Vector2.zero;

                _playerStateManager.ChangeState(_playerStateManager.fallState);
            }
            else if (CheckIfCanWallSlide())
                _playerStateManager.ChangeState(_playerStateManager.wallSlideState);
        }
    }

    private bool CheckIfCanIdle()
    {
        return Mathf.Abs(_playerStateManager.GetDirX()) < 0.1f && _playerStateManager.GetIsOnGround();
    }

    private bool CheckIfCanRun()
    {
        return Mathf.Abs(_playerStateManager.GetDirX()) > 0.1f && _playerStateManager.GetIsOnGround(); 
    }

    private bool CheckIfCanFall()
    {
        return _playerStateManager.GetRigidBody2D().velocity.y < -0.1f;
    }

    private bool CheckIfCanWallSlide()
    {
        //Chạm tường và tích 2 vector inputX và Nx của wall trái dấu
        return _playerStateManager.GetIsWallTouch()
            && _playerStateManager.GetDirX() * _playerStateManager.WallHit.normal.x < 0f;
    }

    private void HandleDash()
    {
        //Vô hiệu hoá grav khi dash (cho ảo hơn)
        _playerStateManager.GetRigidBody2D().gravityScale = 0f;
        _playerStateManager.GetDashSound().Play();
        _playerStateManager.GetTrailRenderer().emitting = true; //Ch thấy effect đâu @@?
        _playerStateManager.gameObject.layer = LayerMask.NameToLayer(GameConstants.IGNORE_ENEMIES_LAYER);
        _playerStateManager.IsDashing = true;

        //Set thẳng thằng velo luôn cho khỏi bị override
        //Vì nếu set theo addforce thì lúc fall nó sẽ dash dần xuống 1 đoạn
        //Chứ 0 phải dash thẳng trên 0 1 đoạn

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
            _playerStateManager.FlipSpriteAfterWallSlide();
    }

    public override void FixedUpdate()
    {
        //nothing
    }

}
