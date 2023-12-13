using UnityEngine;

public class RunState : PlayerBaseState
{
    private bool _hasChanged;

    public override void EnterState(PlayerStateManager playerStateManager)
    {
        base.EnterState(playerStateManager);
        playerStateManager.GetAnimator().SetInteger("state", (int)EnumState.EPlayerState.run);
        playerStateManager.GetDustPS().Play();
        //Debug.Log("Run, Interact? :" + _playerStateManager.IsInteractingWithNPC);
    }

    public override void ExitState() { _hasChanged = false; }

    public override void Update()
    {
        LogicUpdate();
        //Debug.Log("Updateee");
    }

    private void LogicUpdate()
    {
        if (!_playerStateManager.IsInteractingWithNPC)
        {
            if (CheckIfIdle())
            {
                _playerStateManager.ChangeState(_playerStateManager.idleState);
                Debug.Log("Idle Over Here");
            }
            else if (CheckIfJump())
            {
                _playerStateManager.jumpState.IsRunStateHitWall = _playerStateManager.GetIsWallTouch();
                _playerStateManager.ChangeState(_playerStateManager.jumpState);
            }
            else if (CheckIfFall())
                _playerStateManager.ChangeState(_playerStateManager.fallState);
        }
        else if (_playerStateManager.IsInteractingWithNPC && !_hasChanged)
        {
            if (Mathf.Abs(_playerStateManager.transform.position.x - _playerStateManager.InteractPosition.x) < 0.05f)
            {
                _hasChanged = true;
                _playerStateManager.ChangeState(_playerStateManager.idleState);
                Debug.Log("Idle here");
            }
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
                _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetSpeedX() * _playerStateManager.GetDirX(), _playerStateManager.GetRigidBody2D().velocity.y);
        }
        else
        {
            if (_playerStateManager.GetIsFacingRight())
                _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetSpeedX(), _playerStateManager.GetRigidBody2D().velocity.y);
            else
                _playerStateManager.GetRigidBody2D().velocity = new Vector2(-_playerStateManager.GetSpeedX(), _playerStateManager.GetRigidBody2D().velocity.y);
        }
    }
}
