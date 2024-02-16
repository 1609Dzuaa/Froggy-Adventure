using UnityEngine;

public class IdleState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager playerStateManager)
    {
        base.EnterState(playerStateManager);
        _playerStateManager.GetAnimator().SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EPlayerState.idle);
        _playerStateManager.GetRigidBody2D().velocity = Vector2.zero; //Cố định vị trí
        HandleIfInteractWithNPC();
        HandleIfPrevStateWallSlide();

        Debug.Log("Idle"); //Keep this, use for debugging change state
    }

    public override void ExitState() { }

    public override void Update()
    {
        //Chỉ Update State này khi 0 tương tác với NPC, tránh bị loạn state
        if(!_playerStateManager.IsInteractingWithNPC)
        {
            if (CheckIfCanRun())
                _playerStateManager.ChangeState(_playerStateManager.runState);
            else if (CheckIfCanJump())
                _playerStateManager.ChangeState(_playerStateManager.jumpState);
            else if (CheckIfCanFall())
                _playerStateManager.ChangeState(_playerStateManager.fallState);
            else if (CheckIfCanDash())
                _playerStateManager.ChangeState(_playerStateManager.dashState);        }
    }

    private bool CheckIfCanRun()
    {
        //Interacting NPC Prob here
        return _playerStateManager.GetDirX() != 0 && _playerStateManager.GetIsOnGround();
    }

    private bool CheckIfCanJump()
    {
        return Input.GetButtonDown(GameConstants.JUMP_BUTTON) && _playerStateManager.CanJump;
    }

    private bool CheckIfCanFall()
    {
        return _playerStateManager.GetDirY() == 0 && !_playerStateManager.GetIsOnGround() && !_playerStateManager.CanJump;
        //Idle => Fall có thể là đứng yên, bị 1 vật khác
        //tác dụng lực vào đẩy rơi xuống dưới
    }

    private bool CheckIfCanDash()
    {
        //Debug.Log("Dashed?: " + _playerStateManager.dashState.IsFirstTimeDash);
        return Input.GetButtonDown(GameConstants.DASH_BUTTON)
             && Time.time - _playerStateManager.dashState.DashDelayStart >= _playerStateManager.GetPlayerStats.DelayDashTime
             || Input.GetButtonDown(GameConstants.DASH_BUTTON) && _playerStateManager.dashState.IsFirstTimeDash;
    }

    private void HandleIfInteractWithNPC()
    {
        //Nếu đang tương tác với NPC thì flip sprite về hướng NPC
        if (_playerStateManager.IsInteractingWithNPC)
            if (!_playerStateManager.HasDetectedNPC)
            {
                _playerStateManager.FlippingSprite();
                Debug.Log("!Detected, Must Flip");
            }
    }

    private void HandleIfPrevStateWallSlide()
    {
        //State WS sẽ có đặc biệt chút là sẽ flip sprite ngược với isFacingRight
        //Ta sẽ check nếu biến bool prev State là WS thì flip ngược lại
        //và set lại biến đó sau khi flip xong 
        if (_playerStateManager.GetPrevStateIsWallSlide())
            _playerStateManager.FlipSpriteAfterWallSlide();
    }

    public override void FixedUpdate() { }

}