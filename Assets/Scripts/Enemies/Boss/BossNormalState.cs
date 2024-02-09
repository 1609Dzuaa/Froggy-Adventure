using UnityEngine;

public class BossNormalState : CharacterBaseState
{
    BossStateManager _bossManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        _bossManager = (BossStateManager)charactersManager;
        _bossManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EBossState.idleShield);
        _bossManager.GetRigidbody2D().velocity = Vector2.zero;
        //Debug.Log("NormalState");
    }

    public override void ExitState()
    {
        
    }

    public override void Update()
    {
        FlipTowardsPlayer();

        if (CheckIfCanChase())
            _bossManager.ChangeState(_bossManager.ChargeState);
        else if (CheckIfCanSummon())
            _bossManager.ChangeState(_bossManager.SummonState);
    }

    private void FlipTowardsPlayer()
    {
        if (_bossManager.transform.position.x > _bossManager.PlayerRef.position.x && _bossManager.GetIsFacingRight())
            _bossManager.FlippingSprite();
        else if (_bossManager.transform.position.x < _bossManager.PlayerRef.position.x && !_bossManager.GetIsFacingRight())
            _bossManager.FlippingSprite();
    }

    private bool CheckIfCanChase()
    {
        return _bossManager.HasDetectedPlayer && !_bossManager.EnterBattle;
    }

    private bool CheckIfCanSummon()
    {
        return _bossManager.HasDetectedPlayer;
    }

    public override void FixedUpdate() { }
}
