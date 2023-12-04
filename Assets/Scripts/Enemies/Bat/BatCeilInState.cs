using UnityEngine;

public class BatCeilInState : MEnemiesBaseState
{
    private BatManager _batManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _mEnemiesManager.Animator.SetInteger("state", (int)EnumState.BatState.ceilIn);
        _batManager = (BatManager)charactersManager;
        _batManager.BatPatrolState.AllowBackToSleepPos = false;
        Debug.Log("CI");
    }

    public override void ExitState() { }

    public override void Update() { }

    public override void FixedUpdate() { }
}
