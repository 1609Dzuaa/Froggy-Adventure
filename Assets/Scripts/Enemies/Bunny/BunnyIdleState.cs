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
        //prob here
        if (CheckIfCanPatrol())
            _bunnyManager.ChangeState(_bunnyManager.BunnyPatrolState);
        else if (CheckIfCanAttack())
        {
            _hasChangedState = true;
            if (_bunnyManager.IsPlayerBackWard)
                _bunnyManager.FlippingSprite();
            _bunnyManager.Invoke("AllowAttackPlayer", _bunnyManager.EnemiesSO.AttackDelay);
        }
    }

    protected override bool CheckIfCanAttack()
    {
        return _bunnyManager.HasDetectedPlayer && !_hasChangedState
            || _bunnyManager.IsPlayerBackWard && !_hasChangedState;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
