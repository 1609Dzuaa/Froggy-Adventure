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
        }
        else
        {
            //Vì thực sự để so hiệu x = 0 thì rất khó => sử dụng hằng số với giá trị rất nhỏ
            if (Mathf.Abs(_playerStateManager.transform.position.x - _playerStateManager.InteractPosition.x) < GameConstants.STARTCONVERSATIONRANGE)
                _playerStateManager.ChangeState(_playerStateManager.idleState);
        }
    }

    private bool CheckIfIdle()
    {
        if (_playerStateManager.GetDirX() == 0)
            return true;
        return false;
    }

    private bool CheckIfJump()
    {
        //Phải OnGround thì mới cho nhảy
        if (_playerStateManager.GetDirY() < 0 && _playerStateManager.GetIsOnGround())
            return true;
        return false;
    }

    private bool CheckIfFall()
    {
        if (!_playerStateManager.GetIsOnGround())
            return true;
        return false;
        //Idle => Fall có thể là đứng yên, bị 1 vật khác
        //tác dụng lực vào đẩy rơi xuống dưới
    }

    public override void FixedUpdate()
    {
        if (!_playerStateManager.IsInteractingWithNPC)
        {
            if (_playerStateManager.GetDirX() != 0)
                _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetPlayerStats.SPEED_X * _playerStateManager.GetDirX(), _playerStateManager.GetRigidBody2D().velocity.y);
        }
        else
        {
            if (_playerStateManager.GetIsFacingRight())
                _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetPlayerStats.SPEED_X, _playerStateManager.GetRigidBody2D().velocity.y);
            else
                _playerStateManager.GetRigidBody2D().velocity = new Vector2(-_playerStateManager.GetPlayerStats.SPEED_X, _playerStateManager.GetRigidBody2D().velocity.y);
        }
    }
}
