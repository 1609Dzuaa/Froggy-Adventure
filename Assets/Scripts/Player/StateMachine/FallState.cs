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
        //Lỗi fall khi đang trượt hết tường mà dirX != nxWall thì bị kẹt luôn ở cái wall đó
        //DONE!~
    }

    public override void ExitState() { }

    public override void Update()
    {
        //Because the velocity value will not always exactly equal 0
        //So we check does it greater or smaller than a very small value

        if (CheckIfCanIdle())
            _playerStateManager.ChangeState(_playerStateManager.idleState);
        else if (CheckIfCanRun())
            _playerStateManager.ChangeState(_playerStateManager.runState);
        else if (CheckIfCanDbJump())
            _playerStateManager.ChangeState(_playerStateManager.doubleJumpState);
        else if (CheckIfCanWallSlide())
            _playerStateManager.ChangeState(_playerStateManager.wallSlideState);
        else if (CheckIfCanDash())
            _playerStateManager.ChangeState(_playerStateManager.dashState);
        //Debug.Log("Im still here");
    }

    private bool CheckIfCanIdle()
    {
        //Nếu vận tốc 2 trục rất nhỏ VÀ đang trên nền thì coi như đang Idle
        if (Math.Abs(_playerStateManager.GetRigidBody2D().velocity.x) < 0.1f && Math.Abs(_playerStateManager.GetRigidBody2D().velocity.y) < 0.1f && _playerStateManager.GetIsOnGround())
            return true;
        return false;
    }

    private bool CheckIfCanRun()
    {
        //Nếu vận tốc trục x lớn hơn .1f và trục y rất nhỏ
        //và đang OnGround thì chuyển sang state Run
        if (Math.Abs(_playerStateManager.GetRigidBody2D().velocity.x) > 0.1f && Math.Abs(_playerStateManager.GetRigidBody2D().velocity.y) < 0.1f && _playerStateManager.GetIsOnGround())
            return true;
        return false;
    }

    private bool CheckIfCanDbJump()
    {
        //Cho phép lúc Fall có thể Double Jump đc
        if (Input.GetKeyDown(KeyCode.S) && _playerStateManager.GetCanDbJump()) 
            return true;
        return false;
    }

    private bool CheckIfCanWallSlide()
    {
        if (_playerStateManager.GetIsWallTouch() && _playerStateManager.GetDirX() * _playerStateManager.WallHit.normal.x < 0f)
            return true;
        return false;
    }

    private bool CheckIfCanDash()
    {
        //Debug.Log("Dashed?: " + _playerStateManager.dashState.IsFirstTimeDash);
        return Input.GetKeyDown(KeyCode.E)
             && Time.time - _playerStateManager.dashState.DashDelayStart >= _playerStateManager.GetPlayerStats.DelayDashTime
             || Input.GetKeyDown(KeyCode.E) && _playerStateManager.dashState.IsFirstTimeDash;
    }

    public override void FixedUpdate()
    {
        //Vì bị ngu nên ở code cũ nhân thằng Speed với DirX mà 0 check DirX != 0 nên
        //Nhảy đáp từ tường xuống Ground trông đéo được mượt :D
        if (_playerStateManager.GetDirX() != 0)
            if (!PlayerSpeedBuff.Instance.IsAllowToUpdate)
                _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetPlayerStats.SpeedX * _playerStateManager.GetDirX(), _playerStateManager.GetRigidBody2D().velocity.y);
            else
                _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetPlayerStats.SpeedX * PlayerSpeedBuff.Instance.SpeedMultiplier * _playerStateManager.GetDirX(), _playerStateManager.GetRigidBody2D().velocity.y);
    }
}
