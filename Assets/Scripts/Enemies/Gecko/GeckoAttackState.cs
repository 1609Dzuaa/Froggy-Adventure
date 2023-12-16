using UnityEngine;

public class GeckoAttackState : MEnemiesAttackState
{
    private GeckoManager _geckoManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _geckoManager = (GeckoManager)charactersManager;
    }

    public override void ExitState() { }

    public override void Update()
    {

    }

    public override void FixedUpdate() { }
}
