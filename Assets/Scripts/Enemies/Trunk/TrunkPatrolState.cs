using UnityEngine;

public class TrunkPatrolState : MEnemiesPatrolState
{
    TrunkManager _trunkManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _trunkManager = (TrunkManager)charactersManager;
        _trunkManager.Animator.SetInteger("state", (int)EnumState.ETrunkState.patrol);
        Debug.Log("Patrol");
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
        else if (CheckIfCanRetreat())
        {
            _trunkManager.ChangeState(_trunkManager.GetTrunkWithState());
        }
        //base.Update();
    }

    private bool CheckIfCanRetreat()
    {
        return _trunkManager.CanWithDrawn;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
