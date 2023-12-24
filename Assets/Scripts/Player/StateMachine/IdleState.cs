using UnityEngine;

public class IdleState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager playerStateManager)
    {
        base.EnterState(playerStateManager);
        _playerStateManager.GetAnimator().SetInteger("state", (int)EnumState.EPlayerState.idle);
        _playerStateManager.GetRigidBody2D().velocity = Vector2.zero; //Cố định vị trí
        HandleIfInteractWithNPC();
        HandleIfPrevStateWallSlide();

        Debug.Log("Idle, OG: " + _playerStateManager.GetIsOnGround()); //Keep this, use for debugging change state
    }

    public override void ExitState() { }

    public override void Update()
    {
        //Chỉ Update State này khi 0 tương tác với NPC, tránh bị loạn state
        if(!_playerStateManager.IsInteractingWithNPC)
        {
            if (CheckIfRun())
                _playerStateManager.ChangeState(_playerStateManager.runState);
            else if (CheckIfJump())
                _playerStateManager.ChangeState(_playerStateManager.jumpState);
            else if (CheckIfFall())
                _playerStateManager.ChangeState(_playerStateManager.fallState);
            else if (CheckIfCanDash())
                _playerStateManager.ChangeState(_playerStateManager.dashState);        }
    }

    private bool CheckIfRun()
    {
        //Interacting NPC Prob here
        if (_playerStateManager.GetDirX() != 0 && _playerStateManager.GetIsOnGround())
            return true;
        return false;
    }

    private bool CheckIfJump()
    {
        if (_playerStateManager.GetDirY() < 0 && _playerStateManager.GetIsOnGround())
            return true;
        return false;
    }

    private bool CheckIfFall()
    {
        if (_playerStateManager.GetDirY() == 0 && !_playerStateManager.GetIsOnGround())
            return true;
        return false;
        //Idle => Fall có thể là đứng yên, bị 1 vật khác
        //tác dụng lực vào đẩy rơi xuống dưới
    }

    private bool CheckIfCanDash()
    {
        //Debug.Log("Dashed?: " + _playerStateManager.dashState.IsFirstTimeDash);
        return Input.GetKeyDown(KeyCode.E)
             && Time.time - _playerStateManager.dashState.DashDelayStart >= _playerStateManager.GetPlayerStats.DelayDashTime
             || Input.GetKeyDown(KeyCode.E) && _playerStateManager.dashState.IsFirstTimeDash;
    }

    private void HandleIfInteractWithNPC()
    {
        //Nếu đang tương tác với NPC thì flip sprite về hướng NPC
        if (_playerStateManager.IsInteractingWithNPC)
            if (!_playerStateManager.HasDetectedNPC)
            {
                _playerStateManager.FlippingSprite();
                //Debug.Log("!Detected, Must Flip");
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