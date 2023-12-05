using UnityEngine;

public class BunnyPatrolState : MEnemiesPatrolState
{
    private BunnyManager _bunnyManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _bunnyManager = (BunnyManager)charactersManager;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void Update()
    {
        if (CheckIfCanRest())
            _bunnyManager.ChangeState(_bunnyManager.BunnyIdleState);
        else if (CheckIfCanAttack())
            _bunnyManager.Invoke("AllowAttackPlayer", _bunnyManager.GetAttackDelay());
        else if (CheckIfCanChangeDirection())
        {
            _hasChangeDirection = true;
            _bunnyManager.FlippingSprite();
        }
    }

    protected override bool CheckIfCanAttack()
    {
        if (_bunnyManager.HasDetectedPlayer && !_hasChangedState
             || _bunnyManager.IsPlayerBackWard && !_hasChangedState)
        {
            _hasChangedState = true;
            if (_bunnyManager.IsPlayerBackWard) _bunnyManager.FlippingSprite();
            return true;
        }
        return false;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
