using UnityEngine;

public class NMEnemiesAttackState : NMEnemiesBaseState
{
    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _nmEnemiesManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.ENMEnemiesState.attack);
        //Debug.Log("Attack");
    }

    public override void ExitState() { }

    public override void Update() 
    {
        if (!_nmEnemiesManager.HasDetectedPlayer)
            _nmEnemiesManager.ChangeState(_nmEnemiesManager.GetNMEnemiesIdleState);
    }

    public override void FixedUpdate() { }
}
