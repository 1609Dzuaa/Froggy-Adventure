using UnityEngine;
using static GameEnums;

public class BossWallHitState : MEnemiesBaseState
{
    BossStateManager _bossManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        _bossManager = (BossStateManager)charactersManager;
        _bossManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)EBossState.shieldRunOut);
        _bossManager.GetRigidbody2D().velocity = Vector2.zero;
        EventsManager.Instance.NotifyObservers(EEvents.CameraOnShake, null);
        SoundsManager.Instance.PlaySfx(ESoundName.BossWallHitSfx, 1.0f);
        Debug.Log("WH");
    }

    public override void ExitState()
    {
        _bossManager.FlippingSprite();
    }

    public override void Update() { }

    public override void FixedUpdate() { }
}
