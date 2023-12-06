using UnityEngine;

public class IdleState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager playerStateManager)
    {
        base.EnterState(playerStateManager);
        _playerStateManager.GetAnimator().SetInteger("state", (int)EnumState.EPlayerState.idle);

        //State WS sẽ có đặc biệt chút là sẽ flip sprite ngược với isFacingRight
        //Ta sẽ check nếu biến bool prev State là WS thì flip ngược lại
        //và set lại biến đó sau khi flip xong 
        if (_playerStateManager.GetPrevStateIsWallSlide())
            _playerStateManager.FlipSpriteAfterWallSlide();

        Debug.Log("Idle"); //Keep this, use for debugging change state
    }

    public override void ExitState() { }

    public override void Update()
    {
        if (CheckIfRun())
            _playerStateManager.ChangeState(_playerStateManager.runState);
        else if (CheckIfJump())
            _playerStateManager.ChangeState(_playerStateManager.jumpState);
        else if (CheckIfFall())
            _playerStateManager.ChangeState(_playerStateManager.fallState);
        /*UpdateHorizontalLogic();
        UpdateVerticalLogic();*/
    }

    private bool CheckIfRun()
    {
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

    public override void FixedUpdate() { }

}