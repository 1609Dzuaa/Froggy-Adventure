﻿using UnityEngine;

public class HedgehogSpikeIdleState : NMEnemiesBaseState
{
    private HedgehogManager _hedgehogManager;
    private bool _hasChangedState;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _hedgehogManager = (HedgehogManager)charactersManager;
        _hedgehogManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EHedgehogState.spikeIdle);
        _hedgehogManager.getBoxCollider2D.size += _hedgehogManager.getSizeIncrease;
    }

    public override void ExitState()
    {
        base.ExitState();
        _hasChangedState = false;
    }

    public override void Update()
    {
        if (!_hedgehogManager.HasDetectedPlayer && !_hasChangedState)
        {
            _hasChangedState = true;
            _hedgehogManager.Invoke("ChangeToSpikeIn", _hedgehogManager.SpikeInDelay);
        }
        else if (_hedgehogManager.HasDetectedPlayer)
        {
            _hasChangedState = false; //quen mat th nay` =]]
            _hedgehogManager.CancelInvoke(); //Huỷ về Spike In nếu detect ra Player lần nữa
        }
    }
}
