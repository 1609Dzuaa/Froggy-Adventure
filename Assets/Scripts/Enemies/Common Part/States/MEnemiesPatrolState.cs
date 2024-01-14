using UnityEngine;

public class MEnemiesPatrolState : MEnemiesBaseState
{
    protected float _entryTime;
    protected bool _hasChangeDirection = false; //Đảm bảo chỉ flip 1 lần - 0 có thì flip loạn xạ ở đoạn min/max
    protected bool _hasChangedState; //Th này sinh ra nhằm tránh việc Invoke Attack nhiều lần
    protected bool _canRdDirection = true;
    protected bool _hasJustHitWall = false; //Hitwall thì 0 cho Rd hướng
    protected int _rdLeftRight; //0: Left; 1: Right

    public void SetCanRdDirection(bool para) { this._canRdDirection = para; }

    public bool HasChangedDirection { get { return _hasChangeDirection; } set { _hasChangeDirection = value; } }

    //Mọi quái moveable cần func dưới, mục đích đụng tường thì 0 cho rd direction 
    public void SetHasJustHitWall(bool para) { this._hasJustHitWall = para; }

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _mEnemiesManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EMEnemiesState.patrol);
        _entryTime = Time.time;
        //Debug.Log("Patrol, canRD, hasChangeDir, HW: " + _canRdDirection + ", " + _hasChangeDirection + ", " + _hasJustHitWall);
        if (_canRdDirection)
            HandleRandomChangeDirection();
    }

    public override void ExitState()
    {
        //Check trước khi rời state cho lần enter state Patrol tới:
        //Nếu đụng min, max r VÀ chưa hit wall thì lần patrol tiếp 0 đc random
        HandleCanRandomNextPatrolState();
        ResetDataForNextPatrolState();
    }

    public override void Update()
    {
        LogicUpdate();
        //Debug.Log("Pt");
    }

    protected virtual void LogicUpdate()
    {
        //Change States check
        if (CheckIfCanRest())
        {
            _mEnemiesManager.CancelInvoke();
            _mEnemiesManager.ChangeState(_mEnemiesManager.MEnemiesIdleState);
        }
        else if (CheckIfCanAttack())
        {
            _hasChangedState = true;
            _mEnemiesManager.Invoke("AllowAttackPlayer", _mEnemiesManager.EnemiesSO.AttackDelay);
        }

        //Flip Sprite Check
        if (CheckIfCanChangeDirection())
        {
            _hasChangeDirection = true;
            _mEnemiesManager.FlippingSprite();
        }

        Debug.Log("Common Call");
    }

    protected bool CheckIfCanRest()
    {
        return Time.time - _entryTime >= _mEnemiesManager.MEnemiesSO.PatrolTime 
            && !_mEnemiesManager.HasDetectedPlayer;
        //Thêm đk: !_mEnemiesManager.HasDetectedPlayer vì có thể giây cuối idle
    }

    protected virtual bool CheckIfCanAttack()
    {
        return _mEnemiesManager.HasDetectedPlayer && !_hasChangedState;
    }

    protected virtual bool CheckIfCanChangeDirection()
    {
        //ổn r
        return _mEnemiesManager.HasCollidedWall;

        //Check nếu đi quá giới hạn trái/phải, CHƯA đổi hướng ở state này và CHƯA hit wall
        //Thì lật sprite đổi hướng
        //_hasChangeDirection ở đây để lock việc changeDir(chỉ 1 lần duy nhất)
    }

    public override void FixedUpdate()
    {
        Move();
    }

    protected virtual void Move()
    {
        if (_mEnemiesManager.GetIsFacingRight())
            _mEnemiesManager.GetRigidbody2D().velocity = new Vector2(_mEnemiesManager.MEnemiesSO.PatrolSpeed.x, _mEnemiesManager.GetRigidbody2D().velocity.y);
        else
            _mEnemiesManager.GetRigidbody2D().velocity = new Vector2(-_mEnemiesManager.MEnemiesSO.PatrolSpeed.x, _mEnemiesManager.GetRigidbody2D().velocity.y);
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
    }

    private void HandleCanRandomNextPatrolState()
    {
        //Thêm đoạn check bool _hasJustHitWall vì:
        //Lúc hitwall thì hiển nhiên vị trí hitwall lớn hơn min/max |trừ khi bố trí ngu ;)|
        //Dẫn đến phải flip lúc đó -> haschange = true (0 đc rd flip lần sau)
        //TÓM LẠI mình muốn kết quả chạy debug sau khi hit wall như sau:
        //PT1: Rd(false) - HitWall(true)
        //PT2: Rd(true) - Hitwall(false)

        if (_hasChangeDirection && !_hasJustHitWall)
            _canRdDirection = false;
        else
            _canRdDirection = true;
    }

    private void ResetDataForNextPatrolState()
    {
        _hasChangeDirection = false;
        _hasJustHitWall = false;
        _hasChangedState = false;
    }
}
