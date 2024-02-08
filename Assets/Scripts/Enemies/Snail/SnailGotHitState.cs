using UnityEngine;

public class SnailGotHitState : MEnemiesGotHitState
{
    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
    }

    public override void ExitState() { }

    public override void Update() { }

    public override void FixedUpdate() { }

    protected override void HandleBeforeDestroy()
    {
        base.HandleBeforeDestroy();
        _mEnemiesManager.GetRigidbody2D().gravityScale = 1f;
    }
}
