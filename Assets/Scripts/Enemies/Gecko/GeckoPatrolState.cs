using UnityEngine;

public class GeckoPatrolState : MEnemiesPatrolState
{
    private GeckoManager _geckoManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _geckoManager = (GeckoManager)charactersManager;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void Update()
    {
        if (Time.time - _entryTime >= _geckoManager.GetRestTime())
            _geckoManager.ChangeState(_geckoManager.GetGeckoIdleState());
        else if (_geckoManager.HasDetectedPlayer)
            _geckoManager.ChangeState(_geckoManager.GetGeckoHideState());
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
