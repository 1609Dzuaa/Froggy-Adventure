using UnityEngine;

public class MEnemiesPatrolState : MEnemiesBaseState
{
    protected float _entryTime;
    protected bool _hasChangeDirection = false; //Đảm bảo chỉ flip 1 lần
    protected bool _hasChangedState; //Th này sinh ra nhằm tránh việc Invoke Attack nhiều lần
    protected bool _canRdDirection = true;
    protected bool _hasJustHitWall = false; //Hitwall thì 0 cho Rd hướng
    protected int _rdLeftRight; //0: Left; 1: Right

    public void SetCanRdDirection(bool para) { this._canRdDirection = para; }

    public void SetHasChangeDirection(bool para) { _hasChangeDirection = para; }

    //Mọi quái moveable cần func dưới, mục đích đụng tường thì 0 cho rd direction 
    public void SetHasJustHitWall(bool para) { this._hasJustHitWall = para; }

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _mEnemiesManager.Animator.SetInteger("state", (int)EnumState.EMEnemiesState.patrol);
        _entryTime = Time.time;
        if (_canRdDirection)
            HandleRandomChangeDirection();
        //Debug.Log("Patrol");
    }

    public override void ExitState()
    {
        //Check trước khi rời state cho lần enter state Patrol tới:
        //Nếu đụng min, max r thì lần patrol tiếp 0 đc random
        //Debug.Log("PTExitHasChangeDir: " + _hasChangeDirection);
        if (_hasChangeDirection)
            _canRdDirection = false;
        else
            _canRdDirection = true;
        _hasChangeDirection = false;
        _hasJustHitWall = false;
        _hasChangedState = false;
    }

    public override void Update()
    {
        LogicUpdate();
        //Debug.Log("Pt");
    }

    protected virtual void LogicUpdate()
    {
        //recheck this shit
        if (CheckIfCanRest())
        {
            _mEnemiesManager.CancelInvoke();
            _mEnemiesManager.ChangeState(_mEnemiesManager.MEnemiesIdleState);
        }
        else if (CheckIfCanAttack())
        {
            _hasChangedState = true;
            _mEnemiesManager.Invoke("AllowAttackPlayer", _mEnemiesManager.GetAttackDelay());
        }
        else if (CheckIfCanChangeDirection())
        {
            _hasChangeDirection = true;
            _mEnemiesManager.FlippingSprite();
            //Debug.Log("Flip Patrol, Has Hit Wall: " + _hasJustHitWall);
        }
    }

    protected bool CheckIfCanRest()
    {
        return Time.time - _entryTime >= _mEnemiesManager.GetPatrolTime() 
            && !_mEnemiesManager.HasDetectedPlayer;
        //Thêm đk: !_mEnemiesManager.HasDetectedPlayer vì có thể giây cuối idle
    }

    protected virtual bool CheckIfCanAttack()
    {
        return _mEnemiesManager.HasDetectedPlayer && !_hasChangedState;
    }

    protected virtual bool CheckIfCanChangeDirection()
    {
        //recheck
        //Hàm này có vấn đề 

        return _mEnemiesManager.transform.position.x >= _mEnemiesManager.BoundaryRight.position.x && !_hasChangeDirection && !_hasJustHitWall
            || _mEnemiesManager.transform.position.x <= _mEnemiesManager.BoundaryLeft.position.x && !_hasChangeDirection && !_hasJustHitWall;
        //Check nếu đi quá giới hạn trái/phải và CHƯA đổi hướng ở state này
        //Thì lật sprite đổi hướng
        //Thêm ĐK hasJustHitWall vì lúc Hitwall thì hiển nhiên x > x min/max
        //Với ĐK test là min max gần với tường :v
        //Cần xem lại hàm này sau!
    }

    public override void FixedUpdate()
    {
        _mEnemiesManager.Move(_mEnemiesManager.GetPatrolSpeed().x);
    }

    protected virtual void HandleRandomChangeDirection()
    {
        _rdLeftRight = Random.Range(0, 2);
        if (_rdLeftRight == 1 && !_mEnemiesManager.GetIsFacingRight())
            _mEnemiesManager.FlippingSprite();
        else if (_rdLeftRight == 0 && _mEnemiesManager.GetIsFacingRight())
            _mEnemiesManager.FlippingSprite();
        //Random change direction
        //Các TH 0 thể Rd:
        //Vừa tông vào wall => bắt buộc phải flip và patrol hướng ngược lại vs tường
        //Vừa flip vì vượt quá min, max boundaries
    }
}
