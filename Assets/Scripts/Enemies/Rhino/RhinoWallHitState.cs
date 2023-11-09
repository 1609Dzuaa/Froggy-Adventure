using UnityEngine;

public class RhinoWallHitState : BaseState
{
    private bool allowUpdate = false;
    private bool hasHitWall = false;

    public bool GetHasHitWall() { return this.hasHitWall; }

    public void SetAllowUpdate() { this.allowUpdate = true; }

    public override void EnterState(BaseStateManager _baseStateManager)
    {
        if (_baseStateManager is RhinoStateManager)
        {
            rhinoStateManager.GetAnimator().SetInteger("state", (int)EnumState.ERhinoState.wallHit);
            allowUpdate = false;
            hasHitWall = true;
            rhinoStateManager.Invoke("SetTrueWallHitUpdate", 0.5f);
            //Debug.Log("WH"); 
        }
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
            rhinoStateManager.ChangeState(rhinoStateManager.rhinoIdleState);
            rhinoStateManager.FlippingSprite();
        }
    }

    public override void FixedUpdate()
    {

    }
}
