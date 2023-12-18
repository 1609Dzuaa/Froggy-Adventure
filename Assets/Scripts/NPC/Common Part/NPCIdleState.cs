using UnityEngine;

public class NPCIdleState : CharacterBaseState
{
    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _charactersManager.Animator.SetInteger("state", (int)EnumState.ENPCState.idle);
        _charactersManager.GetRigidbody2D().velocity = Vector2.zero;
    }

    public override void ExitState() { }

    public override void Update() { }

    public override void FixedUpdate() { }
}
