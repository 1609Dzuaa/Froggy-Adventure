using UnityEngine;

public class RhinoWallHitState : RhinoBaseState
{
    private bool allowUpdate = false;
    private bool hasHitWall = false;

    public bool GetHasHitWall() { return this.hasHitWall; }

    public void SetAllowUpdate() { this.allowUpdate = true; }

    public override void EnterState(RhinoStateManager rhinoStateManager)
    {
        base.EnterState(rhinoStateManager);
        _rhinoStateManager.GetAnimator().SetInteger("state", (int)EnumState.ERhinoState.wallHit);
        allowUpdate = false;
        hasHitWall = true;
        _rhinoStateManager.Invoke("SetTrueWallHitUpdate", 0.5f);
        //Debug.Log("WH"); 
    }

    public override void ExitState()
    {
        hasHitWall = false;
    }

    public override void UpdateState()
    {
        //Delay nhằm mục đích chạy hết animation WallHit
        if(allowUpdate)
        {
            _rhinoStateManager.FlippingSprite();
            _rhinoStateManager.ChangeState(_rhinoStateManager.rhinoIdleState);
        }
    }

    public override void FixedUpdate()
    {

    }
}
