using System;
using UnityEngine;

public class WallSlideState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager playerStateManager)
    {
        base.EnterState(playerStateManager);
        _playerStateManager.GetAnimator().SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EPlayerState.wallSlide);
        //Debug.Log("WS");
        //Flip sprite khi chuyển từ state này sang state bất kì
        //Theo đúng chiều của nhân vật khi đang slide
        //WS còn vài chỗ lăn tăn nhỏ nữa nhưng chắc thế này là đủ ^^
        //Sau cần thiết thì record fix sau
        //Vẫn còn lỗi nếu isFr != nxWall và bấm S lần đầu sẽ Jump nhưng lúc Jump đó bấm S tiếp
        //thì 0 change state khác đc @@
        //WS vẫn còn bug bên wall trái ?
    }

    public override void ExitState() { }

    public override void Update()
    {
        //Lúc slide wall xuống thì:
        //nếu chạm đất và 0 move thì change sang idle
        //nếu bấm S thì change sang nhảy
        //nếu slide hết tường mà vẫn trên không
        //hoặc directionX cùng dấu với pháp tuyến x của Wall thì fall
        //nếu bấm E thì dash
        //nếu 0 thuộc các TH trên thì slide như bthg

        if (CheckIfCanIdle())
            _playerStateManager.ChangeState(_playerStateManager.idleState);
        else if (CheckIfCanWallJump())
            _playerStateManager.ChangeState(_playerStateManager.wallJumpState);
        else if (CheckIfCanFall())
            _playerStateManager.ChangeState(_playerStateManager.fallState);
        else if (CheckIfCanDash())
            _playerStateManager.ChangeState(_playerStateManager.dashState);
    }

    private bool CheckIfCanIdle()
    {
        return Math.Abs(_playerStateManager.GetRigidBody2D().velocity.x) < .1f
            && Math.Abs(_playerStateManager.GetRigidBody2D().velocity.y) < .1f
            && _playerStateManager.GetIsOnGround();
    }

    private bool CheckIfCanWallJump()
    {
        return Input.GetButtonDown("Jump") && !_playerStateManager.GetIsOnGround();
    }

    private bool CheckIfCanFall()
    {
        //Hiểu đơn giản là pháp tuyến X của Wall mà
        //cùng dấu với directionX
        //hoặc trượt hết tường mà vẫn ở trên không thì Fall
        return _playerStateManager.WallHit.normal.x * _playerStateManager.GetDirX() > 0
            && !Input.GetKeyDown(KeyCode.S) && !_playerStateManager.GetIsOnGround()
            || !_playerStateManager.GetIsOnGround() && !_playerStateManager.GetIsWallTouch()
            && _playerStateManager.GetRigidBody2D().velocity.y < -.1f;
    }

    private bool CheckIfCanDash()
    {
        return Input.GetKeyDown(KeyCode.E)
             && Time.time - _playerStateManager.dashState.DashDelayStart >= _playerStateManager.GetPlayerStats.DelayDashTime
             || Input.GetKeyDown(KeyCode.E) && _playerStateManager.dashState.IsFirstTimeDash;
    }

    public override void FixedUpdate()
    {
        //0 đổi v trục x khi WS, tránh bị nhích ra khỏi wall
        _playerStateManager.GetRigidBody2D().velocity = new Vector2(0f, -_playerStateManager.GetPlayerStats.WallSlideSpeed);
    }
}