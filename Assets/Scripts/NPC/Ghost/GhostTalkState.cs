using UnityEngine;

public class GhostTalkState : CharacterBaseState
{
    private GhostManager _ghostManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _ghostManager = (GhostManager)charactersManager;
        _ghostManager.Animator.SetInteger("state", (int)EnumState.EGhostState.appear);
        _ghostManager.GetRigidbody2D().velocity = Vector2.zero;
        HandleInteractWithPlayer();
        Debug.Log("Talk");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void Update()
    {
        base.Update();
    }

    private void HandleInteractWithPlayer()
    {
        var playerScript = _ghostManager.PlayerRef.GetComponent<PlayerStateManager>();
        if (_ghostManager.GetIsFacingRight() == playerScript.GetIsFacingRight())
            _ghostManager.FlippingSprite();

        _ghostManager.GetDialog().StartDialog();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
