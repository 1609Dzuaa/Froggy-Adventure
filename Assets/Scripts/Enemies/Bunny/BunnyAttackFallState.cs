using System;
using UnityEngine;

public class BunnyAttackFallState : MEnemiesAttackState
{
    private BunnyManager _bunnyManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _bunnyManager = (BunnyManager)charactersManager;
        _bunnyManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EBunnyState.attackFall);
    }

    public override void ExitState() { }

    public override void Update() 
    {
        if (CheckIfCanIdle())
            _bunnyManager.ChangeState(_bunnyManager.BunnyIdleState);
    }

    private bool CheckIfCanIdle()
    {
        return Math.Abs(_bunnyManager.GetRigidbody2D().velocity.x) < 0.1f 
            && Math.Abs(_bunnyManager.GetRigidbody2D().velocity.y) < 0.1f;
    }

    public override void FixedUpdate() { }

}
