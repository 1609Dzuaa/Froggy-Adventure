using UnityEngine;

public class PigAttackGreenState : MEnemiesAttackState
{
    private PigManager _pigManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        _mEnemiesManager = (MEnemiesManager)charactersManager;
        _pigManager = (PigManager)charactersManager;
        _pigManager.Animator.SetInteger("state", (int)EnumState.EPigState.attackGreen);
        Debug.Log("Atk Green");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void Update()
    {
        
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Attack()
    {
        base.Attack();
    }
}
