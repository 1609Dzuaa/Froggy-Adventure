using UnityEngine;

public class MrRunState : BaseState
{
    private bool allowUpdate = false;
    private bool hasChangeState = false;

    public void SetTrueUpdateMrRunState() { this.allowUpdate = true; }
    public override void EnterState(BaseStateManager _baseStateManager)
    {
        if (_baseStateManager is MushroomStateManager)
        {
            mushroomStateManager.GetAnimator().SetInteger("state", (int)EnumState.EMushroomState.run);
            mushroomStateManager.Invoke("AllowUpdateMrRunState", 0.15f); //Delay việc Update trễ 1 khoảng ngắn
            //Nhằm tạo thgian lật sprite bỏ chạy và tránh thoả mãn đk dưới quá nhanh
            //Debug.Log("Run");
        }
    }

    public override void ExitState()
    {
        allowUpdate = false;
        hasChangeState = false;
    }

    public override void UpdateState()
    {
        if (allowUpdate)
        {
            if (!mushroomStateManager.GetIsDetected() && !hasChangeState)
            {
                if (!mushroomStateManager.GetHasCollidedWall())
                {
                    //Debug.Log("Can RD");
                    mushroomStateManager.mrIdleState.SetCanRdDirection(true);
                }
                else
                    mushroomStateManager.FlippingSprite();
                hasChangeState = true;
                mushroomStateManager.ChangeState(mushroomStateManager.mrIdleState);
            }
        }
    }

    public override void FixedUpdate()
    {
        if (mushroomStateManager.GetIsFacingRight())
            mushroomStateManager.GetRigidBody2D().velocity = new Vector2(mushroomStateManager.GetRunSpeed(), mushroomStateManager.GetRigidBody2D().velocity.y);
        else
            mushroomStateManager.GetRigidBody2D().velocity = new Vector2(-mushroomStateManager.GetRunSpeed(), mushroomStateManager.GetRigidBody2D().velocity.y);
    }

}
