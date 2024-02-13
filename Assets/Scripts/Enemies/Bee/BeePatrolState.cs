﻿using UnityEngine;

public class BeePatrolState : MEnemiesPatrolState
{
    private BeeManager _beeManager;
    private float _yOffset;

    public bool HasChangedDirection { set { _hasChangeDirection = value; } }

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _beeManager = (BeeManager)charactersManager;
        _beeManager.MustAttack = false; //Về state này r thì tha cho Player
        _yOffset = Random.Range(-_beeManager.YOffSet, _beeManager.YOffSet);
        //Debug.Log("Bee PT");
    }

    public override void ExitState()
    {
        base.ExitState();
        _hasChangeDirection = false;
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
        return _beeManager.transform.position.x >= _beeManager.BoundaryRight.position.x && !_hasChangeDirection
        || _beeManager.transform.position.x <= _beeManager.BoundaryLeft.position.x && !_hasChangeDirection;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Move()
    {
        if (_beeManager.GetIsFacingRight())
            _beeManager.GetRigidbody2D().velocity = new Vector2(_beeManager.MEnemiesSO.PatrolSpeed.x, _beeManager.MEnemiesSO.PatrolSpeed.y * _yOffset);
        else
            _beeManager.GetRigidbody2D().velocity = new Vector2(-_beeManager.MEnemiesSO.PatrolSpeed.x, _beeManager.MEnemiesSO.PatrolSpeed.y * _yOffset);
    }
}
