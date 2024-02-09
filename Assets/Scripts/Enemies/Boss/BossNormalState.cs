using UnityEngine;

public class BossNormalState : CharacterBaseState
{
    BossStateManager _bossManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        _bossManager = (BossStateManager)charactersManager;
        _bossManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EBossState.idleShield);
        _bossManager.GetRigidbody2D().velocity = Vector2.zero;
        _bossManager.WeakState.IsFirstEnterState = true; //Reset cho Weak State
        //Debug.Log("NormalState");
    }

    public override void ExitState()
    {
        
    }

    public override void Update()
    {
        if (CheckIfCanChase())
            _bossManager.ChangeState(_bossManager.ChargeState);
        else if (CheckIfCanSummon())
            _bossManager.ChangeState(_bossManager.SummonState);
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
