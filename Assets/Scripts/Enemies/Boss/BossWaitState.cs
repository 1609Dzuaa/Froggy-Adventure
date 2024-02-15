using UnityEngine;

public class BossWaitState : MEnemiesBaseState
{
    BossStateManager _bossManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _bossManager = (BossStateManager)charactersManager;
        _bossManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EBossState.idleShield);
        //Debug.Log("GH");
    }

    public override void ExitState() { }

    public override void Update()
    {
        if (_bossManager.EnterBattle && _bossManager.HasDetectedPlayer)
            _bossManager.ChangeState(_bossManager.ChargeState);
    }

    public override void FixedUpdate() { }
}
