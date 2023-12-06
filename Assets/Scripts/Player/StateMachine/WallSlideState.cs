using System;
using UnityEngine;

public class WallSlideState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager playerStateManager)
    {
        base.EnterState(playerStateManager);
        _playerStateManager.GetAnimator().SetInteger("state", (int)EnumState.EPlayerState.wallSlide);
        Debug.Log("WS");
        //Flip sprite khi chuyển từ state này sang state bất kì
        //Theo đúng chiều của nhân vật khi đang slide
        //Lỗi khi đè dirX khiến nó != nxWall dẫn đến loạn State
        //DONE!~
        
    }

    public override void ExitState() { }

    public override void Update()
    {
        //Lúc slide wall xuống thì:
        //nếu chạm đất và 0 move thì change sang idle
        //nếu bấm S thì change sang nhảy
        //nếu slide hết tường mà vẫn trên không
        //hoặc directionX cùng dấu với pháp tuyến x của Wall thì fall
        //nếu 0 thuộc các TH trên thì slide như bthg

        if (CheckIfCanIdle())
            _playerStateManager.ChangeState(_playerStateManager.idleState);
        else if (CheckIfCanWallJump())
            _playerStateManager.ChangeState(_playerStateManager.wallJumpState);
        else if (CheckIfCanFall())
            _playerStateManager.ChangeState(_playerStateManager.fallState);
    }

    private bool CheckIfCanIdle()
    {
        if (Math.Abs(_playerStateManager.GetRigidBody2D().velocity.x) < .1f
            && Math.Abs(_playerStateManager.GetRigidBody2D().velocity.y) < .1f 
            && _playerStateManager.GetIsOnGround())
            return true;
        return false;
    }

    private bool CheckIfCanWallJump()
    {
        if (Input.GetKeyDown(KeyCode.S) && !_playerStateManager.GetIsOnGround())
            return true;
        return false;
    }

    private bool CheckIfCanFall()
    {
        //Hiểu đơn giản là pháp tuyến X của Wall mà
        //cùng dấu với directionX
        //hoặc trượt hết tường mà vẫn ở trên không thì Fall
        //Tại sao 0 ss normal.x == dirX ?
        //=>Vì nhấn dirX nó (Tăng/Giảm Dần) từ 0 -> 1 hoặc -1
        if (_playerStateManager.WallHit.normal.x * _playerStateManager.GetDirX() > 0 
            && !Input.GetKeyDown(KeyCode.S) && !_playerStateManager.GetIsOnGround()
            || !_playerStateManager.GetIsOnGround() && !_playerStateManager.GetIsWallTouch() 
            && _playerStateManager.GetRigidBody2D().velocity.y < -.1f)
            return true;
        return false;
    }

    public override void FixedUpdate()
    {
        //0 đổi v trục x khi WS, tránh bị nhích ra khỏi wall
        _playerStateManager.GetRigidBody2D().velocity = new Vector2(0f, -_playerStateManager.GetWallSlideSpeed());
    }
}