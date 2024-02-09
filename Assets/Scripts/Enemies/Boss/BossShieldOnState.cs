using UnityEngine;

public class BossShieldOnState : CharacterBaseState
{
    BossStateManager _bossManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        _bossManager = (BossStateManager)charactersManager;
        _bossManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EBossState.hitShieldOn);
        Debug.Log("Shield On");
    }

    public override void ExitState() { }

    public override void Update() { }

    public override void FixedUpdate() { }
}
