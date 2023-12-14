using UnityEngine;

public class TrunkAttackState : MEnemiesAttackState
{
    private TrunkManager _trunkManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _trunkManager = (TrunkManager)charactersManager;
        _trunkManager.GetRigidbody2D().velocity = Vector2.zero;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        
    }

    protected override void Attack()
    {
        
    }
}
