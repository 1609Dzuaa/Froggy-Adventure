using UnityEngine;

public class BeePatrolState : MEnemiesPatrolState
{
    private BeeManager _beeManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _beeManager = (BeeManager)charactersManager;
        _beeManager.MustAttack = false; //Về state này r thì tha cho Player
        //Debug.Log("Bee Pt: " + _canRdDirection + ", " + _hasChangeDirection);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void Update()
    {
        if (CheckIfCanRest())
            _beeManager.ChangeState(_beeManager.GetBeeIdleState());
        else if (CheckIfCanChase())
            _beeManager.ChangeState(_beeManager.GetBeeChaseState());
        else if (CheckIfCanChangeDirection())
        {
            _hasChangeDirection = true;
            _beeManager.FlippingSprite();
        }
    }

    private bool CheckIfCanChase()
    {
        return _beeManager.HasDetectedPlayer;
    }

    protected override bool CheckIfCanChangeDirection()
    {
        if (_beeManager.transform.position.x >= _beeManager.BoundaryRight.position.x && !_hasChangeDirection
            || _beeManager.transform.position.x <= _beeManager.BoundaryLeft.position.x && !_hasChangeDirection)
            return true;
        return false;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
