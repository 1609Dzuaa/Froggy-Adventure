using UnityEngine;

public class GhostIdleState : NPCIdleState
{
    private GhostManager _ghostManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        _ghostManager = (GhostManager)charactersManager;
        _ghostManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EGhostState.idle);
        //Debug.Log("G Idle");
    }

    public override void ExitState() { }

    public override void Update()
    {
        if (!_ghostManager.GetIsPlayerNearBy())
            _ghostManager.ChangeState(_ghostManager.GetGhostDisappearState());
        FlipTowardPlayer();
    }

    private void FlipTowardPlayer()
    {
        if (_ghostManager.transform.position.x < _ghostManager.PlayerReference.transform.position.x && !_ghostManager.GetIsFacingRight()
            || _ghostManager.transform.position.x > _ghostManager.PlayerReference.transform.position.x && _ghostManager.GetIsFacingRight())
            _ghostManager.FlippingSprite();
    }

    public override void FixedUpdate() { }
}
