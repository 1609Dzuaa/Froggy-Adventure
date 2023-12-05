using UnityEngine;

public class BunnyIdleState : MEnemiesIdleState
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
        if (CheckIfCanPatrol())
            _bunnyManager.ChangeState(_bunnyManager.BunnyPatrolState);
        else if (CheckIfCanAttack())
            _bunnyManager.Invoke("AllowAttackPlayer", _bunnyManager.GetAttackDelay());
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
