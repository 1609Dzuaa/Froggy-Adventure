using UnityEngine;
using static GameEnums;

public class GotHitState : PlayerBaseState
{
    private bool _isHitByTrap; //Nếu bị hit bởi Traps thì mới AddForce dựa vào hướng mặt của Player
    private float _entryTime = 0; //Để đếm giờ lúc bị hit phục vụ cho việc miễn Dmg

    public bool IsHitByTrap { set { _isHitByTrap = value; } }

    public float EntryTime { get {  return _entryTime; } }

    public override void EnterState(PlayerStateManager playerStateManager)
    {
        base.EnterState(playerStateManager);
        HandleGotHit();
        if (PlayerHealthManager.Instance.CurrentHP > 0)
            _playerStateManager.GetAnimator().SetInteger(GameConstants.ANIM_PARA_STATE, (int)EPlayerState.gotHit);
        //Debug.Log("GotHit");

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
        //if (!BuffsManager.Instance.GetBuff(EBuffs.Absorb).IsActivating)
        {
            if (_isHitByTrap)
            {
                if (_playerStateManager.GetIsFacingRight())
                    _playerStateManager.GetRigidBody2D().AddForce(new Vector2(-1 * _playerStateManager.GetPlayerStats.KnockBackForce.x, 0f));
                else
                    _playerStateManager.GetRigidBody2D().AddForce(new Vector2(_playerStateManager.GetPlayerStats.KnockBackForce.x, 0f));
            }
            else
            {
                if (_playerStateManager.IsHitFromRightSide)
                    _playerStateManager.GetRigidBody2D().AddForce(new Vector2(_playerStateManager.GetPlayerStats.KnockBackForce.x, 0f));
                else
                    _playerStateManager.GetRigidBody2D().AddForce(new Vector2(-_playerStateManager.GetPlayerStats.KnockBackForce.x, 0f));
            }
        }
    }

    private void HandleGotHit()
    {
        bool hasTempHP = PlayerHealthManager.Instance.HasTempHP;
        EventsManager.NotifyObservers(EEvents.OnChangeHP, hasTempHP ? EHPStatus.MinusOneTempHP : EHPStatus.MinusOneHP);
        if (PlayerHealthManager.Instance.CurrentHP == 0)
            _playerStateManager.HandleDeadState();

        _playerStateManager.IsVunerable = true;
        _entryTime = Time.time;
        KnockBack();
        _playerStateManager.gameObject.layer = LayerMask.NameToLayer(GameConstants.IGNORE_ENEMIES_LAYER);
        _playerStateManager.IsApplyGotHitEffect = true;
        if (PlayerHealthManager.Instance.CurrentHP > 0)
            SoundsManager.Instance.PlaySfx(ESoundName.PlayerGotHitSfx, 1.0f);
        EventsManager.NotifyObservers(EEvents.CameraOnShake);
    }
}
