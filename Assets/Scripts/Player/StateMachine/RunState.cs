using UnityEngine;

public class RunState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager playerStateManager)
    {
        base.EnterState(playerStateManager);
        playerStateManager.GetAnimator().SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EPlayerState.run);
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
            if (CheckIfCanIdle())
                _playerStateManager.ChangeState(_playerStateManager.idleState);
            else if (CheckIfCanJump())
            {
                _playerStateManager.jumpState.IsRunStateHitWall = _playerStateManager.GetIsWallTouch();
                _playerStateManager.ChangeState(_playerStateManager.jumpState);
            }
            else if (CheckIfCanFall())
                _playerStateManager.ChangeState(_playerStateManager.fallState);
            else if (CheckIfCanDash())
                _playerStateManager.ChangeState(_playerStateManager.dashState);
        }
        else
        {
            if (CheckIfNearInteractPosition())
                _playerStateManager.ChangeState(_playerStateManager.idleState); //switch về idle nghe NPC sủa
        }
    }

    private bool CheckIfCanIdle()
    {
        return _playerStateManager.GetDirX() == 0;
    }

    private bool CheckIfCanJump()
    {
        return _playerStateManager.BtnJumpDetect && _playerStateManager.CanJump;
    }

    private bool CheckIfCanFall()
    {
        return !_playerStateManager.GetIsOnGround() && !_playerStateManager.CanJump;
        //Idle => Fall có thể là đứng yên, bị 1 vật khác
        //tác dụng lực vào đẩy rơi xuống dưới
    }

    private bool CheckIfCanDash()
    {
        return Input.GetButtonDown(GameConstants.DASH_BUTTON)
            && Time.time - _playerStateManager.dashState.DashDelayStart >= _playerStateManager.GetPlayerStats.DelayDashTime
            || Input.GetButtonDown(GameConstants.DASH_BUTTON) && _playerStateManager.dashState.IsFirstTimeDash;
    }

    private bool CheckIfNearInteractPosition()
    {
        //Vì thực sự để so hiệu x = 0 thì rất khó => sử dụng hằng số với giá trị rất nhỏ

        return Mathf.Abs(_playerStateManager.transform.position.x - _playerStateManager.InteractPosition.x) < GameConstants.CAN_START_CONVERSATION_RANGE;
    }

    public override void FixedUpdate()
    {
        if (!_playerStateManager.IsInteractingWithNPC)
        {
            if (_playerStateManager.GetDirX() != 0)
            {
                if (!BuffsManager.Instance.GetTypeOfBuff(GameEnums.EBuffs.Speed).IsAllowToUpdate)
                    _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetPlayerStats.SpeedX * _playerStateManager.GetDirX(), _playerStateManager.GetRigidBody2D().velocity.y);
                else
                    _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetPlayerStats.SpeedX * _playerStateManager.GetDirX() * ((PlayerSpeedBuff)BuffsManager.Instance.GetTypeOfBuff(GameEnums.EBuffs.Speed)).SpeedMultiplier, _playerStateManager.GetRigidBody2D().velocity.y);
            }
        }
        else
        {
            if (_playerStateManager.GetIsFacingRight())
                _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetPlayerStats.SpeedX, _playerStateManager.GetRigidBody2D().velocity.y);
            else
                _playerStateManager.GetRigidBody2D().velocity = new Vector2(-_playerStateManager.GetPlayerStats.SpeedX, _playerStateManager.GetRigidBody2D().velocity.y);
            //Debug.Log("here");
        }
    }
}
