using UnityEngine;

public class BossWallHitState : CharacterBaseState
{
    BossStateManager _bossManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        _bossManager = (BossStateManager)charactersManager;
        _bossManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EBossState.shieldRunOut);
        _bossManager.GetRigidbody2D().velocity = Vector2.zero;
        Debug.Log("WH");
    }

    public override void ExitState()
    {
        _bossManager.FlippingSprite();
    }

    public override void Update() { }

    public override void FixedUpdate() { }
}
