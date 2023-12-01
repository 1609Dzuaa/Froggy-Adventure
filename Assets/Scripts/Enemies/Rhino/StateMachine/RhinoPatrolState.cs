using UnityEngine;

public class RhinoPatrolState : RhinoBaseState
{
    private float entryTime;
    private bool allowUpdateState = false; //Delay trễ xíu để chạy animation
    private bool hasChangeDirection = false; //Đảm bảo chỉ flip 1 lần
    private bool canRdDirection = true;
    private bool hasJustHitWall = false; //Hitwall thì 0 cho Rd hướng
    private int rdLeftRight; //0: Left; 1: Right

    public void SetTrueAllowUpdate() { this.allowUpdateState = true; }

    public void SetCanRdDirection(bool para) { this.canRdDirection = para; }

    public void SetHasJustHitWall(bool para) { this.hasJustHitWall = para; }

    //CanRdDirection = false <=> vừa flip ở state này xong
    public override void EnterState(RhinoStateManager rhinoStateManager)
    {
        base.EnterState(rhinoStateManager);
        _rhinoStateManager.GetAnimator().SetInteger("state", (int)EnumState.ERhinoState.patrol);
        allowUpdateState = false;
        entryTime = Time.time;
        if (canRdDirection)
            HandleRandomChangeDirection();
        //Debug.Log("Patrol");
    }

    public override void ExitState()
    {
        hasChangeDirection = false;
    }

    public override void Update()
    {
        CheckChangeDirection();
        if (allowUpdateState)
        {
            if (_rhinoStateManager.GetHasDetectedPlayer())
            {
                hasJustHitWall = false;
                _rhinoStateManager.ChangeState(_rhinoStateManager.rhinoRunState);
            }
            else if (Time.time - entryTime >= _rhinoStateManager.GetPatrolTime())
            {
                //Nếu đổi hướng vì chạm min, max r thì 0 cho phép random
                if (!hasChangeDirection)
                    canRdDirection = true;
                hasJustHitWall = false;
                _rhinoStateManager.ChangeState(_rhinoStateManager.rhinoIdleState);
            }
        }
    }

    public override void FixedUpdate()
    {
        UpdatePhysicsHorizontal();
        //Debug.Log("Can Rd: " + canRdDirection);
    }

    private void UpdatePhysicsHorizontal()
    {
        if (_rhinoStateManager.GetIsFacingRight())
            _rhinoStateManager.GetRigidBody2D().velocity = new Vector2(_rhinoStateManager.GetPatrolSpeed(), _rhinoStateManager.GetRigidBody2D().velocity.y);
        else
            _rhinoStateManager.GetRigidBody2D().velocity = new Vector2(-1 * _rhinoStateManager.GetPatrolSpeed(), _rhinoStateManager.GetRigidBody2D().velocity.y);
    }

    private void CheckChangeDirection()
    {
        if (_rhinoStateManager.transform.position.x > _rhinoStateManager.GetMaxPointRight().position.x
            && !hasChangeDirection && !hasJustHitWall 
            || _rhinoStateManager.transform.position.x < _rhinoStateManager.GetMaxPointLeft().position.x 
            && !hasChangeDirection && !hasJustHitWall)
        {
            hasChangeDirection = true;
            canRdDirection = false;
            _rhinoStateManager.FlippingSprite();
        }
        //Check nếu đi quá giới hạn trái/phải và CHƯA đổi hướng ở state này
        //Thì lật sprite đổi hướng
        //Thêm ĐK hasJustHitWall vì lúc Hitwall thì hiển nhiên x > x min/max
    }

    private void HandleRandomChangeDirection()
    {
        rdLeftRight = Random.Range(0, 2);
        if (rdLeftRight == 1 && !_rhinoStateManager.GetIsFacingRight())
            _rhinoStateManager.FlippingSprite();
        else if (rdLeftRight == 0 && _rhinoStateManager.GetIsFacingRight())
            _rhinoStateManager.FlippingSprite();
        //Random change direction
        //Các TH 0 thể Rd:
        //Vừa tông vào wall => bắt buộc phải flip và patrol hướng ngược lại vs tường
        //Vừa flip vì vượt quá min, max boundaries
    }
}
