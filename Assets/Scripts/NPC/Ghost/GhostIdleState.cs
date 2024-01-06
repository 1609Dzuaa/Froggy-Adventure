using UnityEngine;

public class GhostIdleState : NPCIdleState
{
    private GhostManager _ghostManager;
    private float _entryTime;

    public override void EnterState(CharactersManager charactersManager)
    {
        _ghostManager = (GhostManager)charactersManager;
        _ghostManager.Animator.SetInteger("state", (int)GameEnums.EGhostState.idle);
        _ghostManager.GetRigidbody2D().velocity = Vector2.zero;
        _entryTime = Time.time;
        //Debug.Log("Idle");
    }

    public override void ExitState() { }

    public override void Update()
    {
        if (CheckIfCanWander())
            _ghostManager.ChangeState(_ghostManager.GetGhostWanderState());
    }

    private bool CheckIfCanWander()
    {
        return Time.time - _entryTime >= _ghostManager.RestTime;
    }

    public override void FixedUpdate() { }
}
