using System;
using UnityEngine;

public class FallState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager playerStateManager)
    {
        base.EnterState(playerStateManager);
        _playerStateManager.GetAnimator().SetInteger("state", (int)EnumState.EPlayerState.fall);

        if (_playerStateManager.GetPrevStateIsWallSlide())
            _playerStateManager.FlipSpriteAfterWallSlide();
        //Debug.Log("Fall");
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        UpdateHorizontalLogic();
        //Because the velocity value will not always exactly equal 0
        //So we check does it greater or smaller than a very small value

        //Nếu vận tốc 2 trục rất nhỏ thì coi như đang Idle
        if (Math.Abs(_playerStateManager.GetRigidBody2D().velocity.x) < 0.1f && Math.Abs(_playerStateManager.GetRigidBody2D().velocity.y) < 0.1f)
            _playerStateManager.ChangeState(_playerStateManager.idleState);
        //Nếu vận tốc trục x lớn hơn .1f và trục y rất nhỏ
        //và đang OnGround thì
        //chuyển sang state Run
        //Prob here ?
        else if (Math.Abs(_playerStateManager.GetRigidBody2D().velocity.x) > 0.1f && Math.Abs(_playerStateManager.GetRigidBody2D().velocity.y) < 0.1f && _playerStateManager.GetIsOnGround())
            _playerStateManager.ChangeState(_playerStateManager.runState);
        else if (_playerStateManager.GetIsWallTouch() && !_playerStateManager.GetIsOnGround())
            _playerStateManager.ChangeState(_playerStateManager.wallSlideState);
        //Cho phép lúc Fall có thể Double Jump đc
        else if (Input.GetKeyDown(KeyCode.S) && !_playerStateManager.GetHasDbJump())
            _playerStateManager.ChangeState(_playerStateManager.doubleJumpState);
    }

    void UpdateHorizontalLogic()
    {
        _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetSpeedX() * _playerStateManager.GetDirX(), _playerStateManager.GetRigidBody2D().velocity.y);
    }

    public override void FixedUpdate()
    {

    }
}
