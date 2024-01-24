﻿using UnityEngine;

public class GhostTalkState : NPCTalkState
{
    //còn bug vẫn hiện press T dù start conver
    private GhostManager _ghostManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        //base.EnterState(charactersManager);
        _ghostManager = (GhostManager)charactersManager;
        _ghostManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EGhostState.appear);
        HandleInteractWithPlayer(_ghostManager, _ghostManager.GetStartIndex());
        //Debug.Log("Talk");
    }

    public override void ExitState() { }

    public override void Update()
    {
        //Đéo tiếp chuyện nữa thì trả về Idle State
        if (!_ghostManager.GetDialog().Started)
        {
            //Prob with Jump here
            _ghostManager.ChangeState(_ghostManager.GetGhostIdleState());
        }
    }

    public override void FixedUpdate() { }
}
