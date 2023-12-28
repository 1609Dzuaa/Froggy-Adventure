using UnityEngine;

public class RhinoWallHitState : MEnemiesBaseState
{
    private bool _allowUpdate;
    private RhinoManager _rhinoManager;

    public void AllowUpdate() { this._allowUpdate = true; }

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _rhinoManager = (RhinoManager)charactersManager;
        _rhinoManager.Animator.SetInteger("state", (int)EnumState.ERhinoState.wallHit);
        _rhinoManager.MEnemiesPatrolState.SetCanRdDirection(false);
        _rhinoManager.MEnemiesPatrolState.SetHasJustHitWall(true);
        Debug.Log("WH");
    }

    public override void ExitState()
    {
        _allowUpdate = false;
        _rhinoManager.IsHitShield = false;
    }

    public override void Update()
    {
        //Delay nhằm mục đích chạy hết animation WallHit
        if(_allowUpdate)
            HandleAfterHitWall();
    }

    public override void FixedUpdate() { }

    private void HandleAfterHitWall()
    {
        if (!_rhinoManager.IsHitShield)
            _rhinoManager.FlippingSprite();
        _rhinoManager.ChangeState(_rhinoManager.MEnemiesIdleState);
    }
}
