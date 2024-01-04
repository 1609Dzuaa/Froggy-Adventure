using UnityEngine;

public class SnailIdleState : MEnemiesIdleState
{
    private SnailManager _snailManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _snailManager = (SnailManager)charactersManager;
        //Reset 2 thg dưới cho Attack
        _snailManager.BoxCol2D.size = _snailManager.OriginBoxSize;
        _snailManager.BoxCol2DTrigger.offset = _snailManager.OriginOffset;
        //  Debug.Log("Idle");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void Update()
    {
        if (CheckIfCanPatrol())
            _snailManager.ChangeState(_snailManager.SnailPatrolState);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
