using UnityEngine;

public class TrunkIdleState : MEnemiesIdleState
{
    TrunkManager _trunkManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _trunkManager = (TrunkManager)charactersManager;
        _trunkManager.Animator.SetInteger("state", (int)GameEnums.ETrunkState.idle);
        //Debug.Log("Idle");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void Update()
    {
        if (CheckIfCanPatrol())
        {
            _trunkManager.CancelInvoke();
            _trunkManager.ChangeState(_trunkManager.GetTrunkPatrolState());
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
