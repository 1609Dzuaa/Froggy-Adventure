using UnityEngine;

public class GotHitState : PlayerBaseState
{
    private bool _isHitByTrap; //Nếu bị hit bởi Traps thì mới AddForce dựa vào hướng mặt của Player
    private float _entryTime = 0;

    public bool IsHitByTrap { set { _isHitByTrap = value; } }

    public float EntryTime { get {  return _entryTime; } }

    public override void EnterState(PlayerStateManager playerStateManager)
    {
        base.EnterState(playerStateManager);
        HandleGotHit();
        if (PlayerHealthController.Instance.CurrentHP > 0)
            _playerStateManager.GetAnimator().SetInteger("state", (int)EnumState.EPlayerState.gotHit);
        //Debug.Log("GotHit");

        //Change layer trong đây để enemies có thể đâm xuyên qua khi đang bị thương ?
        //Chú ý khi làm việc với Any State
        //Tắt Transition To Self ở đoạn nối Transition từ Any State tới State cụ thể
        //Tránh bị đứng ngay frame đầu tiên
    }

    public override void ExitState() 
    { 
        _isHitByTrap = false;
    }

    //Phải handle change state trong đây chứ th animation event 0 đảm nhận việc đó đc
    public override void Update() { }

    public override void FixedUpdate() { }

    private void KnockBack()
    {
        if (_playerStateManager.GetIsFacingRight())
            _playerStateManager.GetRigidBody2D().AddForce(new Vector2(-1 * _playerStateManager.GetPlayerStats.KnockBackForce.x, 0f));
        else
            _playerStateManager.GetRigidBody2D().AddForce(new Vector2(_playerStateManager.GetPlayerStats.KnockBackForce.x, 0f));
        //Debug.Log("Knock");
    }

    private void HandleGotHit()
    {
        if (!PlayerAbsorbBuff.Instance.IsAllowToUpdate)
            PlayerHealthController.Instance.ChangeHPState(GameConstants.HP_STATE_LOST);
        else
            PlayerHealthController.Instance.ChangeHPState(GameConstants.HP_STATE_TEMP);
        
        if (PlayerHealthController.Instance.CurrentHP == 0)
        {
            //Xử lý bay màu ở đây là hợp lý
            _playerStateManager.HandleDeadState();
            return;
        }

        _entryTime = Time.time;
        if (_isHitByTrap)
            KnockBack();
        _playerStateManager.gameObject.layer = LayerMask.NameToLayer(GameConstants.IGNORE_ENEMIES_LAYER);
        _playerStateManager.IsApplyGotHitEffect = true;
        SoundsManager.Instance.GetTypeOfSound(GameConstants.PLAYER_GOT_HIT_SOUND).Play();
    }
}
