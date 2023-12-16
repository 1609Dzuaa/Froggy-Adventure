using UnityEngine;

public class GeckoHideState : MEnemiesBaseState
{
    private GeckoManager _geckoManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _geckoManager = (GeckoManager)charactersManager;
        _geckoManager.Animator.SetInteger("state", (int)EnumState.EGeckoState.hide);
        //Thêm Spawn effect khi Hide
    }

    public override void ExitState() { }

    public override void Update() { }

    public override void FixedUpdate() { }
}
