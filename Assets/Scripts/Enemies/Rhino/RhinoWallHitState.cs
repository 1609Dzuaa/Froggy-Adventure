using UnityEngine;

public class RhinoWallHitState : MEnemiesBaseState
{
    private RhinoManager _rhinoManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _rhinoManager = (RhinoManager)charactersManager;
        _rhinoManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.ERhinoState.wallHit);
        _rhinoManager.MEnemiesPatrolState.CanRdDirection = false;
        _rhinoManager.MEnemiesPatrolState.SetHasJustHitWall(true);
        SoundsManager.Instance.PlaySfx(GameEnums.ESoundName.RhinoWallHitSfx, 1.0f);
        //Debug.Log("WH");
    }

    public override void ExitState()
    {
        _rhinoManager.IsHitShield = false;
    }

    public override void Update() { }

    public override void FixedUpdate() { }
}
