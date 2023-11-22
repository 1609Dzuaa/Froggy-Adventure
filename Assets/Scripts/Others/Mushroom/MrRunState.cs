using UnityEngine;

public class MrRunState : MrBaseState
{
    private bool allowUpdate = false;
    private bool hasChangeState = false;

    public void SetTrueUpdateMrRunState() { this.allowUpdate = true; }
    public override void EnterState(MrStateManager mrStateManager)
    {
        base.EnterState(mrStateManager);
        _mrStateManager.GetAnimator().SetInteger("state", (int)EnumState.EMushroomState.run);
        _mrStateManager.Invoke("AllowUpdateMrRunState", 0.15f); //Delay việc Update trễ 1 khoảng ngắn
        //Nhằm tạo thgian lật sprite bỏ chạy và tránh thoả mãn đk dưới quá nhanh
        //Debug.Log("Run");
    }

    public override void ExitState()
    {
        allowUpdate = false;
        hasChangeState = false;
    }

    public override void Update()
    {
        if (allowUpdate)
        {
            if (!_mrStateManager.GetIsDetected() && !hasChangeState)
            {
                if (!_mrStateManager.GetHasCollidedWall())
                {
                    //Debug.Log("Can RD");
                    _mrStateManager.mrIdleState.SetCanRdDirection(true);
                }
                else
                    _mrStateManager.FlippingSprite();
                hasChangeState = true;
                _mrStateManager.ChangeState(_mrStateManager.mrIdleState);
            }
        }
    }

    public override void FixedUpdate()
    {
        if (_mrStateManager.GetIsFacingRight())
            _mrStateManager.GetRigidBody2D().velocity = new Vector2(_mrStateManager.GetRunSpeed(), _mrStateManager.GetRigidBody2D().velocity.y);
        else
            _mrStateManager.GetRigidBody2D().velocity = new Vector2(-_mrStateManager.GetRunSpeed(), _mrStateManager.GetRigidBody2D().velocity.y);
    }

}
