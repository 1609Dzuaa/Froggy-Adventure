using UnityEngine;

public class RhinoWallHitState : MEnemiesBaseState
{
    private bool _allowUpdate = false;
    private RhinoManager _rhinoManager;

    public void SetAllowUpdate() { this._allowUpdate = true; }

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        //_rhinoManager.Animator.SetInteger("state", EnumState.EMEnemiesState.)
        _rhinoManager = (RhinoManager)charactersManager;
    }

    public override void ExitState()
    {
        
    }

    public override void Update()
    {
        //Delay nhằm mục đích chạy hết animation WallHit
        if(_allowUpdate)
            HandleAfterHitWall();
    }

    public override void FixedUpdate()
    {

    }

    private void HandleAfterHitWall()
    {
        _rhinoManager.FlippingSprite();
        _rhinoManager.ChangeState(_rhinoManager.MEnemiesIdleState);
    }
}
