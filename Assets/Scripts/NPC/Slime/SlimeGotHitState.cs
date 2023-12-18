using UnityEngine;

public class SlimeGotHitState : CharacterBaseState
{
    private SlimeManager _slimeManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _slimeManager = (SlimeManager)charactersManager;
        _slimeManager.Animator.SetInteger("state", (int)EnumState.ESlimState.gotHit);
        _slimeManager.GetDialog().StartDialog(_slimeManager.GetStartIndexIfGotHit());
    }

    public override void ExitState() { }

    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate() { }
}
