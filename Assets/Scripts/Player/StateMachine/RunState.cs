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

    public override void Update()
    {
        LogicUpdate();
    }

    private void LogicUpdate()
    {
        if (CheckIfIdle())
            _playerStateManager.ChangeState(_playerStateManager.idleState);
        else if (CheckIfJump())
            _playerStateManager.ChangeState(_playerStateManager.jumpState);
        else if (CheckIfFall())
            _playerStateManager.ChangeState(_playerStateManager.fallState);
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

    void PhysicsUpdate()
    {
        if (_playerStateManager.GetDirX() != 0)
        {
            _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetSpeedX() * _playerStateManager.GetDirX(), _playerStateManager.GetRigidBody2D().velocity.y);
        }
        //Like function's name. Only update things related to Physics
    }

    public override void FixedUpdate()
    {
        PhysicsUpdate();
    }
}
