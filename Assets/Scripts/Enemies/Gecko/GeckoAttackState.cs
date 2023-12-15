using UnityEngine;

public class GeckoAttackState : MEnemiesAttackState
{
    private GeckoManager _geckoManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _geckoManager = (GeckoManager)charactersManager;
    }

    public override void ExitState() { _geckoManager.GetBoxCollider2D().enabled = false; }

    public override void Update()
    {

    }

    public override void FixedUpdate() { }
}
