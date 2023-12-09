using UnityEngine;

public class NMEnemiesAttackState : NMEnemiesBaseState
{
    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _nmEnemiesManager.Animator.SetInteger("state", (int)EnumState.ENMEnemiesState.attack);
        //Debug.Log("Attack");
    }

    public override void ExitState() { }

    public override void Update() 
    {
        if (!_nmEnemiesManager.HasDetectedPlayer)
            _nmEnemiesManager.ChangeState(_nmEnemiesManager.getNMEnemiesIdleState);
    }

    public override void FixedUpdate() { }
}
