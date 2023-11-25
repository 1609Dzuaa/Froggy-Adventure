using UnityEngine;

public class RhinoWallHitState : RhinoBaseState
{
    private bool allowUpdate = false;

    //Detect Player đôi lúc bị chậm(Detected nhưng chưa chuyển state)
    //Đâm tường thì 0 cho random
    //Vấn đề ở đoạn flip sprite min, max Point

    public void SetAllowUpdate() { this.allowUpdate = true; }

    public override void EnterState(RhinoStateManager rhinoStateManager)
    {
        base.EnterState(rhinoStateManager);
        _rhinoStateManager.GetAnimator().SetInteger("state", (int)EnumState.ERhinoState.wallHit);
        allowUpdate = false;
        _rhinoStateManager.rhinoPatrolState.SetHasJustHitWall(true);
        _rhinoStateManager.rhinoPatrolState.SetCanRdDirection(false); //Đụng tường thì 0 cho Rd hướng ở patrol
        //Debug.Log("WH"); 
    }

    public override void ExitState()
    {
        
    }

    public override void Update()
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
