using UnityEngine;

public class RhinoPatrolState : RhinoBaseState
{
    private float startPos = 0;
    private bool allowUpdate = false;
    public void SetTrueAllowUpdate() { this.allowUpdate = true; }

    public override void EnterState(RhinoStateManager rhinoStateManager)
    {
        base.EnterState(rhinoStateManager);
        _rhinoStateManager.GetAnimator().SetInteger("state", (int)EnumState.ERhinoState.patrol);
        startPos = _rhinoStateManager.transform.position.x;
        allowUpdate = false;
        _rhinoStateManager.Invoke("SetTruePatrolUpdate", 0.3f); //call get func instead of hard-coding
        //Delay 1 khoảng 0.3s sau khi vào state patrol để
        //tránh tình trạng quay mặt rồi run ngay lập tức!
        //Patrol animation is actually the same as Run animation but at a half frame-rate lower:)
        //Debug.Log("Patrol");
    }

    public override void ExitState()
    {

    }

    public override void Update()
    {
        if(allowUpdate)
        {
            if (_rhinoStateManager.GetHasDetectedPlayer())
                _rhinoStateManager.ChangeState(_rhinoStateManager.rhinoRunState);
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
        if (_rhinoStateManager.GetIsFacingRight())
            _rhinoStateManager.GetRigidBody2D().velocity = new Vector2(_rhinoStateManager.GetPatrolSpeed(), _rhinoStateManager.GetRigidBody2D().velocity.y);
        else
            _rhinoStateManager.GetRigidBody2D().velocity = new Vector2(-1 * _rhinoStateManager.GetPatrolSpeed(), _rhinoStateManager.GetRigidBody2D().velocity.y);
    }

    private void CheckWhetherNeedRest()
    {
        if (_rhinoStateManager.GetIsFacingRight())
        {
            if (_rhinoStateManager.transform.position.x > _rhinoStateManager.GetPatrolDistance() + startPos 
                && !_rhinoStateManager.GetHasCollidedWall())
            {
                //Bổ sung thêm đk: !rhinoStateManager.GetHasCollidedWall()
                //Vì có thể có TH: thoả đk if này và collide với wall nên vẫn có thể
                //random direction đc
                _rhinoStateManager.ChangeState(_rhinoStateManager.rhinoIdleState);
                _rhinoStateManager.rhinoIdleState.SetCanRdDirection(true);
                //Debug.Log("ChangeFR");
            }
            else if(_rhinoStateManager.GetHasCollidedWall())
            {
                _rhinoStateManager.ChangeState(_rhinoStateManager.rhinoIdleState);
                _rhinoStateManager.FlippingSprite();
            }
        }
        else
        {
            if (_rhinoStateManager.transform.position.x < startPos - _rhinoStateManager.GetPatrolDistance()
                && !_rhinoStateManager.GetHasCollidedWall())
            {
                _rhinoStateManager.ChangeState(_rhinoStateManager.rhinoIdleState);
                _rhinoStateManager.rhinoIdleState.SetCanRdDirection(true);
                //Debug.Log("ChangeFL");
            }
            else if (_rhinoStateManager.GetHasCollidedWall())
            {
                _rhinoStateManager.ChangeState(_rhinoStateManager.rhinoIdleState);
                _rhinoStateManager.FlippingSprite();
            }
        }
    }
}
