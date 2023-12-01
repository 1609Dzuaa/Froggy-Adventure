using UnityEngine;

public class RhinoRunState : RhinoBaseState
{
    private bool hasChangedState = false;
    public override void EnterState(RhinoStateManager rhinoStateManager)
    {
        base.EnterState(rhinoStateManager);
        _rhinoStateManager.GetAnimator().SetInteger("state", (int)EnumState.ERhinoState.run);
        //Dùng Instantiate tạo Prefab Warning
        rhinoStateManager.SpawnWarning();

        //Debug.Log("Run");
    }

    public override void ExitState()
    {
        hasChangedState = false;
    }

    public override void Update()
    {
        if (_rhinoStateManager.GetHasCollidedWall())
        {
            //Ưu tiên switch state WH hơn Idle khi đang Run
            hasChangedState = true;
            //Xoá Invoke func vì có thể đã invoke Idle ở dưới nhưng lại đâm tường ở đây
            _rhinoStateManager.CancelInvoke();
            //_rhinoStateManager.ChangeState(_rhinoStateManager.rhinoWallHitState);
        }
        else if(!_rhinoStateManager.GetHasDetectedPlayer() && !hasChangedState)
        {
            hasChangedState = true;
            //Debug.Log("Here");
            _rhinoStateManager.Invoke("ChangeToIdle", _rhinoStateManager.GetRestDelay());
        }
    }

    void UpdateHorizontalLogic()
    {
        if (_rhinoStateManager.GetIsFacingRight())
            _rhinoStateManager.GetRigidBody2D().velocity = new Vector2(_rhinoStateManager.GetRunSpeed(), _rhinoStateManager.GetRigidBody2D().velocity.y);
        else
            _rhinoStateManager.GetRigidBody2D().velocity = new Vector2(-1 * _rhinoStateManager.GetRunSpeed(), _rhinoStateManager.GetRigidBody2D().velocity.y);
    }

    public override void FixedUpdate()
    {
        UpdateHorizontalLogic();
    }
}
