using UnityEngine;

public class TrunkGotHitState : MEnemiesGotHitState
{
    TrunkManager _trunkManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        _mEnemiesManager = (MEnemiesManager)charactersManager;
        _trunkManager = (TrunkManager)charactersManager;
        _trunkManager.Animator.SetInteger("state", (int)GameEnums.ETrunkState.gotHit);
        HandleBeforeDestroy();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void Update()
    {
        base.Update();
    }
}
