using UnityEngine;

public class GeckoAttackState : MEnemiesAttackState
{
    private GeckoManager _geckoManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _geckoManager = (GeckoManager)charactersManager;
        _geckoManager.GetRigidbody2D().velocity = Vector2.zero;
    }

    public override void ExitState() { }

    public override void Update() { }

    public override void FixedUpdate() { }
}
