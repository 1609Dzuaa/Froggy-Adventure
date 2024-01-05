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

    public override void ExitState() { }

    public override void Update()
    {
        LogicUpdate();
    }

    private void LogicUpdate()
    {
        if (!_playerStateManager.IsInteractingWithNPC)
        {
            if (CheckIfIdle())
                _playerStateManager.ChangeState(_playerStateManager.idleState);
            else if (CheckIfJump())
            {
                _playerStateManager.jumpState.IsRunStateHitWall = _playerStateManager.GetIsWallTouch();
                _playerStateManager.ChangeState(_playerStateManager.jumpState);
            }
            else if (CheckIfFall())
                _playerStateManager.ChangeState(_playerStateManager.fallState);
            else if (CheckIfCanDash())
                _playerStateManager.ChangeState(_playerStateManager.dashState);
        }
        else
        {
            //Vì thực sự để so hiệu x = 0 thì rất khó => sử dụng hằng số với giá trị rất nhỏ
            if (Mathf.Abs(_playerStateManager.transform.position.x - _playerStateManager.InteractPosition.x) < GameConstants.START_CONVERSATION_RANGE)
                _playerStateManager.ChangeState(_playerStateManager.idleState);
        }
    }

    private bool CheckIfIdle()
    {
        return _playerStateManager.GetDirX() == 0;
    }

    private bool CheckIfJump()
    {
        return Input.GetButtonDown("Jump") && _playerStateManager.GetIsOnGround();
        //Phải OnGround thì mới cho nhảy
        /*if (_playerStateManager.GetDirY() < 0 && _playerStateManager.GetIsOnGround())
            return true;
        return false;*/
    }

    private bool CheckIfFall()
    {
        if (!_playerStateManager.GetIsOnGround())
            return true;
        return false;
        //Idle => Fall có thể là đứng yên, bị 1 vật khác
        //tác dụng lực vào đẩy rơi xuống dưới
    }

    private bool CheckIfCanDash()
    {
        return Input.GetKeyDown(KeyCode.E)
            && Time.time - _playerStateManager.dashState.DashDelayStart >= _playerStateManager.GetPlayerStats.DelayDashTime
            || Input.GetKeyDown(KeyCode.E) && _playerStateManager.dashState.IsFirstTimeDash;
    }

    public override void FixedUpdate()
    {
        if (!_playerStateManager.IsInteractingWithNPC)
        {
            if (_playerStateManager.GetDirX() != 0)
            {
                if (!PlayerSpeedBuff.Instance.IsAllowToUpdate)
                    _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetPlayerStats.SpeedX * _playerStateManager.GetDirX(), _playerStateManager.GetRigidBody2D().velocity.y);
                else
                    _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetPlayerStats.SpeedX * _playerStateManager.GetDirX() * PlayerSpeedBuff.Instance.SpeedMultiplier, _playerStateManager.GetRigidBody2D().velocity.y);
            }
        }
        else
        {
            if (_playerStateManager.GetIsFacingRight())
                _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetPlayerStats.SpeedX, _playerStateManager.GetRigidBody2D().velocity.y);
            else
                _playerStateManager.GetRigidBody2D().velocity = new Vector2(-_playerStateManager.GetPlayerStats.SpeedX, _playerStateManager.GetRigidBody2D().velocity.y);
        }
    }
}
