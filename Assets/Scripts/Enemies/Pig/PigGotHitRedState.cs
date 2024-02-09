using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigGotHitRedState : MEnemiesGotHitState
{
    private PigManager _pigManager;
    private bool _allowUpdate;

    public bool AllowUpdate { set { _allowUpdate = value; } }

    public override void EnterState(CharactersManager charactersManager)
    {
        _mEnemiesManager = (MEnemiesManager)charactersManager;
        _pigManager = (PigManager)charactersManager;
        _pigManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EPigState.gotHitRed);
        _pigManager.GetRigidbody2D().velocity = Vector2.zero;
        if (_pigManager.HP == 0)
        {
            HandleBeforeDestroy();
            _pigManager.Invoke("SelfDestroy", _pigManager.DestroyDelay);
        }
        //Debug.Log("GH Red");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void Update()
    {
        if (_allowUpdate && _pigManager.HP > 0)
            _pigManager.ChangeState(_pigManager.GetPigAttackRedState());
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
