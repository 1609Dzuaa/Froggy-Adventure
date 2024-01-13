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
        if (Time.time - _entryTime >= _geckoManager.MEnemiesSO.RestTime && !_hasChangedState)
        {
            _hasChangedState = true;
            _geckoManager.ChangeState(_geckoManager.GetGeckoIdleState());
        }
        else if (_geckoManager.HasDetectedPlayer && !_hasChangedState)
        {
            _hasChangedState = true;
            _geckoManager.StartCoroutine(_geckoManager.Hide());
        }
        else if (CheckIfCanChangeDirection())
        {
            _hasChangeDirection = true;
            _mEnemiesManager.FlippingSprite();
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
