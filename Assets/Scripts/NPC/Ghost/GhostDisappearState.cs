using UnityEngine;

public class GhostDisappearState : CharacterBaseState
{
    private GhostManager _ghostManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _ghostManager = (GhostManager)charactersManager;
        _ghostManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EGhostState.disappear);
        //Debug.Log("Dis");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void Update()
    {
        if (CheckIfCanAppear())
            _ghostManager.ChangeState(_ghostManager.GetGhostAppearState());
        //Debug.Log("im hereee");
    }

    private bool CheckIfCanAppear()
    {
        return _ghostManager.GetIsPlayerNearBy();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}