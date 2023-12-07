using UnityEngine;

public class SnailShellHitState : MEnemiesBaseState
{
    private SnailManager _snailManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _snailManager = (SnailManager)charactersManager;
        _snailManager.Animator.SetInteger("state", (int)EnumState.ESnailState.shellHit);
        _snailManager.HealthPoint -= 1;
        _snailManager.SetHasGotHit(false);
    }

    public override void ExitState() { }

    public override void Update() { }

    public override void FixedUpdate() { }
}