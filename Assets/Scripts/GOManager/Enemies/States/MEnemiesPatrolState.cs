using UnityEngine;

public class MEnemiesPatrolState : MEnemiesBaseState
{
    protected float _entryTime;
    protected bool _allowAttack = false; //Delay trễ xíu để chạy animation
    protected bool _hasChangeDirection = false; //Đảm bảo chỉ flip 1 lần
    protected bool _canRdDirection = true;
    protected bool _hasJustHitWall = false; //Hitwall thì 0 cho Rd hướng
    protected int _rdLeftRight; //0: Left; 1: Right

    public void AllowAttack() { this._allowAttack = true; }

    public void SetCanRdDirection(bool para) { this._canRdDirection = para; }

    public void SetHasJustHitWall(bool para) { this._hasJustHitWall = para; }

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _mEnemiesManager.Animator.SetInteger("state", (int)EnumState.EMEnemiesState.patrol);
        _allowAttack = false;
        _entryTime = Time.time;
        if (_canRdDirection)
            HandleRandomChangeDirection();
        Debug.Log("patrol");
    }

    public override void ExitState() { }

    public override void Update()
    {
        if (CheckIfIdle())
            _mEnemiesManager.ChangeState(_mEnemiesManager.MEnemiesIdleState);
        else if (CheckIfCanAttack())
            _mEnemiesManager.ChangeState(_mEnemiesManager.MEnemiesAttackState);
    }

    protected bool CheckIfIdle()
    {
        if (Time.time - _entryTime >= _mEnemiesManager.GetPatrolTime())
            return true;
        return false;
    }

    protected bool CheckIfCanAttack()
    {
        if (_mEnemiesManager.HasDetectedPlayer && _allowAttack)
            return true;
        return false;
    }

    public override void FixedUpdate()
    {
        _mEnemiesManager.Move(_mEnemiesManager.GetPatrolSpeed());
    }

    protected void HandleRandomChangeDirection()
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
