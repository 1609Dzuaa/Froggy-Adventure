using UnityEngine;

public class TrunkPatrolState : MEnemiesPatrolState
{
    TrunkManager _trunkManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _trunkManager = (TrunkManager)charactersManager;
        _trunkManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.ETrunkState.patrol);
        //Debug.Log("Patrol");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void Update()
    {
        if (CheckIfCanRest())
        {
            _trunkManager.CancelInvoke();
            _trunkManager.ChangeState(_trunkManager.GetTrunkIdleState());
        }
        else if (CheckIfCanWithdrawn())
        {
            _trunkManager.ChangeState(_trunkManager.GetTrunkWithState());
        }
    }

    private bool CheckIfCanWithdrawn()
    {
        return _trunkManager.CanWithDrawn;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
