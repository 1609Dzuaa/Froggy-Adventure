using UnityEngine;

public class RunState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager playerStateManager)
    {
        base.EnterState(playerStateManager);
        playerStateManager.GetAnimator().SetInteger("state", (int)EnumState.EPlayerState.run);
        playerStateManager.GetDustPS().Play();
        //Debug.Log("Run");
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        UpdateHorizontalLogic();
        UpdateVerticalLogic();
    }

    void UpdateHorizontalLogic()
    {
        if (_playerStateManager.GetDirX() != 0)
        {
            _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetSpeedX() * _playerStateManager.GetDirX(), _playerStateManager.GetRigidBody2D().velocity.y);
        }
        else //X-direction = 0
        {
            _playerStateManager.ChangeState(_playerStateManager.idleState);
        }
    }

    void UpdateVerticalLogic()
    {
        //Hướng Y khác 0 tức là đang nhảy hoặc rơi
        if (_playerStateManager.GetDirY() < 0)
        {
            if (_playerStateManager.GetIsOnGround())
                _playerStateManager.ChangeState(_playerStateManager.jumpState);
        }
        else if (_playerStateManager.GetRigidBody2D().velocity.y < -0.1f)
        {
            _playerStateManager.ChangeState(_playerStateManager.fallState);
        }
    }

    public override void FixedUpdate()
    {
        UpdateHorizontalLogic();
    }
}
