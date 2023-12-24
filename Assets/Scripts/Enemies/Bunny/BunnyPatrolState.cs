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
        //Th này ra đây hợp lí hơn
        if (CheckIfCanChangeDirection())
        {
            _hasChangeDirection = true;
            _bunnyManager.FlippingSprite();
        }

        if (CheckIfCanRest())
        {
            //Xoá hết Invoke
            _mEnemiesManager.CancelInvoke();
            _bunnyManager.ChangeState(_bunnyManager.BunnyIdleState);
        }
        else if (CheckIfCanAttack())
        {
            _hasChangedState = true;
            _bunnyManager.Invoke("AllowAttackPlayer", _bunnyManager.GetAttackDelay());
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
