using UnityEngine;

public class BossWeakState : MEnemiesIdleState
{
    BossStateManager _bossManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        _bossManager = (BossStateManager)charactersManager;
        _bossManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EBossState.idleNoShield);
        _bossManager.GetRigidbody2D().velocity = Vector2.zero;
        _entryTime = Time.time;
        //Debug.Log("WeakState");
    }

    public override void ExitState() { }

    public override void Update()
    {
        if (CheckIfCanBackToNormal())
            _bossManager.ChangeState(_bossManager.ShieldOnState);
    }

    private bool CheckIfCanBackToNormal()
    {
        return Time.time - _entryTime >= _bossManager.WeakStateTime;
    }

    public override void FixedUpdate() { }
}
