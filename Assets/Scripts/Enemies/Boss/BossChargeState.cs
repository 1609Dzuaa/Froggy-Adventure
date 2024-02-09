using UnityEngine;

//State này chỉ xộc thẳng về 1 phía cho đến khi đâm Wall
public class BossChargeState : MEnemiesAttackState
{
    BossStateManager _bossManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _bossManager = (BossStateManager)charactersManager;
        _bossManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EBossState.idleShield);
        _bossManager.EnterBattle = true;
        //Debug.Log("ChargeState");
    }

    public override void ExitState() { }

    public override void Update()
    {
        if (CheckIfHitWall())
            _bossManager.ChangeState(_bossManager.WallHitState);
    }

    private bool CheckIfHitWall()
    {
        return _bossManager.HasCollidedWall;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
