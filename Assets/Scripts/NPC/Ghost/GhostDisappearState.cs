using UnityEngine;

public class GhostDisappearState : CharacterBaseState
{
    private GhostManager _ghostManager;
    private float _entryTime;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _ghostManager = (GhostManager)charactersManager;
        _ghostManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EGhostState.disappear);
        _ghostManager.GetRigidbody2D().velocity = Vector2.zero;
        _entryTime = Time.time;
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
    }

    private bool CheckIfCanAppear()
    {
        return Time.time - _entryTime >= _ghostManager.DisappearTime;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}