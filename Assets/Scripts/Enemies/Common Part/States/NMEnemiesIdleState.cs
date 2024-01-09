using UnityEngine;

public class NMEnemiesIdleState : NMEnemiesBaseState
{
    private bool _hasChangedState;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _nmEnemiesManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.ENMEnemiesState.idle);
        //Debug.Log("Idle");
    }

    public override void ExitState() { _hasChangedState = false; }

    public override void Update() 
    {
        if (_nmEnemiesManager.HasDetectedPlayer && !_hasChangedState)
        {
            _hasChangedState = true;
            _nmEnemiesManager.Invoke("AllowAttackPlayer", _nmEnemiesManager.GetAttackDelay());
        }
    }

    public override void FixedUpdate() { }
}
