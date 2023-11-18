using Unity.VisualScripting;
using UnityEngine;

public class JumpState : PlayerBaseState
{
    //Considering Coyote Time
    private bool hasChangedState = false;
    public override void EnterState(PlayerStateManager playerStateManager)
    {
        base.EnterState(playerStateManager);
        playerStateManager.GetAnimator().SetInteger("state", (int)EnumState.EPlayerState.jump);
        playerStateManager.GetDustPS().Play();
        HandleJump();
        //Nếu state trước là WS thì tức là đang WallJump
        if (playerStateManager.GetPrevStateIsWallSlide())
        {
            playerStateManager.FlipSpriteAfterWallSlide();
        }
        //Debug.Log("Jump");
        //cung` dau: WS, JUMP, JUMP, WS
        hasChangedState = false;
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        UpdateJumpLogic();
        if (hasChangedState) 
            return;
        UpdateHorizontalLogic();
    }

    private void UpdateJumpLogic()
    {
        //Press S While Jump => Double Jump
        if (Input.GetKeyDown(KeyCode.S))
        {
            _playerStateManager.ChangeState(_playerStateManager.doubleJumpState);
            hasChangedState = true;
        }
        else if (_playerStateManager.GetRigidBody2D().velocity.y < -0.1f)
        {
            //Debug.Log("Change here: " + playerStateManager.GetRigidBody2D().velocity.y);
            _playerStateManager.ChangeState(_playerStateManager.fallState);
            hasChangedState = true;
        }
        //Still Prob Here
        else if (_playerStateManager.GetIsWallTouch() && !_playerStateManager.GetIsOnGround())
        {
            _playerStateManager.ChangeState(_playerStateManager.wallSlideState);
            hasChangedState = true;
        }
    }

    private void HandleJump()
    {
        //Nếu nhảy khi đang trượt tường thì nhảy xéo
        if (!_playerStateManager.GetPrevStateIsWallSlide())
            _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetRigidBody2D().velocity.x, _playerStateManager.GetSpeedY());
        else
        {
            if (_playerStateManager.GetIsFacingRight())
            {
                _playerStateManager.GetRigidBody2D().velocity = new Vector2(-1 * _playerStateManager.GetWallJumpSpeedX(), _playerStateManager.GetWallJumpSpeedY());
                //Vì lúc trượt tường thì chưa set lại IsFR nên ở đây bắt buộc phải nhân -1
                //Debug.Log("FR");
            }
            else
            {
                _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetWallJumpSpeedX(), _playerStateManager.GetWallJumpSpeedY());
                //Debug.Log("vX, vY: " + playerStateManager.GetRigidBody2D().velocity.x +", "+ playerStateManager.GetRigidBody2D().velocity.y);
            }
        }
        _playerStateManager.GetJumpSound().Play();
    }

    private void UpdateHorizontalLogic()
    {
        //Prob here:
        //Khi bay nhưng điều hướng A||D thì lại chui vào đây mất
        //Do hàm FLip after WS khiến WallCheck bị lật ngược lại dẫn đến thế này
        if (_playerStateManager.GetDirX() != 0 )//&& !playerStateManager.GetIsWallTouch())
        {
            //Debug.Log("Here");
            _playerStateManager.GetRigidBody2D().velocity = new Vector2(_playerStateManager.GetSpeedX() * _playerStateManager.GetDirX(), _playerStateManager.GetRigidBody2D().velocity.y);
        }
    }

    public override void FixedUpdate()
    {

    }

}
