using UnityEngine;

public class BeeIdleState : MEnemiesIdleState
{
    private BeeManager _beeManager; 

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _beeManager = (BeeManager)charactersManager;
        //Debug.Log("Bee Idle");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void Update()
    {
        if (CheckIfCanPatrol())
            _beeManager.ChangeState(_beeManager.GetBeePatrolState());
        else if (CheckIfCanChase())
            _beeManager.ChangeState(_beeManager.GetBeeChaseState());
    }

    private bool CheckIfCanChase()
    {
        return _beeManager.HasDetectedPlayer;
    }
}
