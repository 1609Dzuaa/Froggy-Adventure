using UnityEngine;

public class BossGotHitState : MEnemiesBaseState
{
    BossStateManager _bossManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _bossManager = (BossStateManager)charactersManager;
        _bossManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EBossState.hitShieldOff);
        //Debug.Log("GH");
    }

    public override void ExitState() { _bossManager.HasGotHit = false; }

    public override void Update() { }

    public override void FixedUpdate() { }
}
