using UnityEngine;

public class RhinoPatrolState : BaseState
{
    private float startPos = 0;
    private bool allowUpdate = false;
    public void SetTrueAllowUpdate() { this.allowUpdate = true; }

    public override void EnterState(BaseStateManager _baseStateManager)
    {
        if (_baseStateManager is RhinoStateManager)
        {
            rhinoStateManager.GetAnimator().SetInteger("state", (int)EnumState.ERhinoState.patrol);
            startPos = rhinoStateManager.transform.position.x;
            allowUpdate = false;
            rhinoStateManager.Invoke("SetTruePatrolUpdate", 0.3f);
            //Delay 1 khoảng 0.3s sau khi vào state patrol để
            //tránh tình trạng quay mặt rồi run ngay lập tức!
            //Patrol animation is actually the same as Run animation but at a half frame-rate lower:)
            //Debug.Log("Patrol");
        }
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        if(allowUpdate)
        {
            if (rhinoStateManager.GetHasDetectedPlayer())
                rhinoStateManager.ChangeState(rhinoStateManager.rhinoRunState);
            else
                CheckWhetherNeedRest();
        }
    }

    public override void FixedUpdate()
    {
        UpdateHorizontalLogic();
    }

    private void UpdateHorizontalLogic()
    {
        if (rhinoStateManager.GetIsFacingRight())
            rhinoStateManager.GetRigidBody2D().velocity = new Vector2(rhinoStateManager.GetPatrolSpeed(), rhinoStateManager.GetRigidBody2D().velocity.y);
        else
            rhinoStateManager.GetRigidBody2D().velocity = new Vector2(-1 * rhinoStateManager.GetPatrolSpeed(), rhinoStateManager.GetRigidBody2D().velocity.y);
    }

    private void CheckWhetherNeedRest()
    {
        if (rhinoStateManager.GetIsFacingRight())
        {
            if (rhinoStateManager.transform.position.x > rhinoStateManager.GetPatrolDistance() + startPos)
            {
                rhinoStateManager.ChangeState(rhinoStateManager.rhinoIdleState);
                rhinoStateManager.rhinoIdleState.SetCanRdDirection(true);
                //Debug.Log("ChangeFR");
            }
            else if(rhinoStateManager.GetHasCollidedWall())
            {
                rhinoStateManager.ChangeState(rhinoStateManager.rhinoIdleState);
                rhinoStateManager.FlippingSprite();
            }
        }
        else
        {
            if (rhinoStateManager.transform.position.x < startPos - rhinoStateManager.GetPatrolDistance())
            {
                rhinoStateManager.ChangeState(rhinoStateManager.rhinoIdleState);
                rhinoStateManager.rhinoIdleState.SetCanRdDirection(true);
                //Debug.Log("ChangeFL");
            }
            else if (rhinoStateManager.GetHasCollidedWall())
            {
                rhinoStateManager.ChangeState(rhinoStateManager.rhinoIdleState);
                rhinoStateManager.FlippingSprite();
            }
        }
    }
}
