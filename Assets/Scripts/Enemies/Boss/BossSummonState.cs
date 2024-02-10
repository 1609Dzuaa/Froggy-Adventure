using UnityEngine;

public class BossSummonState : CharacterBaseState
{
    BossStateManager _bossManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        _bossManager = (BossStateManager)charactersManager;
        _bossManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EBossState.idleShield);
        _bossManager.GetRigidbody2D().velocity = Vector2.zero;
        _bossManager.StartCoroutine(_bossManager.Slam(0));
        _bossManager.StartCoroutine(_bossManager.BackToNormal());
        Debug.Log("Summon");
    }

    public override void ExitState() { }

    public override void Update() { }

    public override void FixedUpdate() { }
}
