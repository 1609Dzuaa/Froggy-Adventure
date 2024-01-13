using UnityEngine;

public class GeckoIdleState : MEnemiesIdleState
{
    private GeckoManager _geckoManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _geckoManager = (GeckoManager)charactersManager;
    }

    public override void ExitState() { }

    public override void Update()
    {
        if (Time.time - _entryTime >= _geckoManager.MEnemiesSO.RestTime)
            _geckoManager.ChangeState(_geckoManager.GetGeckoPatrolState());
        else if (_geckoManager.HasDetectedPlayer)
            _geckoManager.ChangeState(_geckoManager.GetGeckoHideState());
    }

    public override void FixedUpdate() { }
}
