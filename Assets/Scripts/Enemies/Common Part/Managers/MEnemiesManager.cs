using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public class MEnemiesManager : EnemiesManager
{
    //MEnemies <-> MoveableEnemies
    //Thêm detectGround cho MEnemies -> đỡ phải phụ thuộc bound
    //Cho quái collide với box -> flip hướng (state is Patrol)
    //có thể coi như quái canh box
    //=>muốn ăn item -> phải giết quái
    protected MEnemiesIdleState _mEnemiesIdleState = new();
    protected MEnemiesPatrolState _mEnemiesPatrolState = new();
    protected MEnemiesAttackState _mEnemiesAttackState = new();
    protected MEnemiesGotHitState _mEnemiesGotHitState = new();

    [Header("SO2")]
    [SerializeField] protected MEnemiesStats _mEnemiesSO;

    [Header("Wall Check")]
    [SerializeField] protected Transform _wallCheck;
    protected bool _hasCollidedWall;

    [Header("Ground Check")]
    [SerializeField] protected Transform _groundCheck;
    protected bool _hasDetectedGround;

    #region GETTER

    public MEnemiesIdleState MEnemiesIdleState { get { return _mEnemiesIdleState; } }
    
    public MEnemiesPatrolState MEnemiesPatrolState { get { return _mEnemiesPatrolState;} set { _mEnemiesPatrolState = value; } }

    public bool HasCollidedWall { get { return _hasCollidedWall; } }

    public bool HasDetectedGround { get { return _hasDetectedGround; } }

    public MEnemiesStats MEnemiesSO { get { return _mEnemiesSO; } }
    
    #endregion
    
    protected override void Awake()
    {
        base.Awake(); //Lấy anim, rb từ EnemiesManager
    }

    //Các lớp sau muốn mở rộng hàm Start, Update,... thì nhớ gọi base.Start(),... trong hàm Start của chính nó
    //Còn 0 implement gì thêm thì 0 cần làm gì, nó tự động đc gọi trong đây ngay cả khi là private
    protected override void Start()
    {
        base.Start();
    }

    protected override void SetUpProperties()
    {
        base.SetUpProperties();
        _state = _mEnemiesIdleState;
        _state.EnterState(this);
    }

    // Update is called once per frame
    protected override void Update()
    {
        //Đã bao gồm việc Update state, detect và draw Ray detect Player ở base
        base.Update();
        DrawRayDetectWall();
        DrawRayDetectGround();
        //Debug.Log("Hit Wall: " + _hasCollidedWall);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        DetectWall();
        DetectGround();
    }

    public override void ChangeState(CharacterBaseState state)
    {
        //Chết là hết
        if (_state is MEnemiesGotHitState) 
            return;

        _state.ExitState();
        _state = state;
        _state.EnterState(this);
    }

    protected virtual bool DetectWall()
    {
        if (!_isFacingRight)
            _hasCollidedWall = Physics2D.Raycast(_wallCheck.position, Vector2.left, _mEnemiesSO.WallCheckDistance, _mEnemiesSO.WallLayer);
        else
            _hasCollidedWall = Physics2D.Raycast(_wallCheck.position, Vector2.right, _mEnemiesSO.WallCheckDistance, _mEnemiesSO.WallLayer);
    
        return _hasCollidedWall;
    }

    protected virtual void DrawRayDetectWall()
    {
        if (!_hasDetectedGround)
        {
            if (!_isFacingRight)
                Debug.DrawRay(_wallCheck.position, Vector2.left * _mEnemiesSO.WallCheckDistance, Color.green);
            else
                Debug.DrawRay(_wallCheck.position, Vector2.right * _mEnemiesSO.WallCheckDistance, Color.green);
        }
        else
        {
            if (!_isFacingRight)
                Debug.DrawRay(_wallCheck.position, Vector2.left * _mEnemiesSO.WallCheckDistance, Color.red);
            else
                Debug.DrawRay(_wallCheck.position, Vector2.right * _mEnemiesSO.WallCheckDistance, Color.red);
        }
    }

    protected virtual void DetectGround()
    {
        _hasDetectedGround = Physics2D.Raycast(_groundCheck.position, Vector2.down, _mEnemiesSO.GroundCheckDistance, _mEnemiesSO.WallLayer);
    }

    protected virtual void DrawRayDetectGround()
    {
        if (!_hasDetectedGround)
            Debug.DrawRay(_groundCheck.position, Vector2.down * _mEnemiesSO.GroundCheckDistance, Color.green);
        else
            Debug.DrawRay(_groundCheck.position, Vector2.down * _mEnemiesSO.GroundCheckDistance, Color.red);
    }

    //Hàm này dùng để Invoke khi detect ra Player
    protected virtual void AllowAttackPlayer()
    {
        if (BuffsManager.Instance.GetBuff(EBuffs.Invisible).IsActivating)
        {
            ChangeState(_mEnemiesIdleState);
            return;
        }

        ChangeState(_mEnemiesAttackState);
        //Debug.Log("Called");
        //Nhằm delay việc chuyển state Attack 
        //Tạo cảm giác enemies phản ứng rồi attack chứ 0 phải attack ngay lập tức
    }

    protected virtual void ChangeToIdle()
    {
        ChangeState(_mEnemiesIdleState);
        //Dùng để Invoke trong state Attack nếu 0 detect ra player
        //Với Mushroom thì 0 detect ra ở sau lưng
        //Với Rhino thì 0 detect ra trước mặt
        //Với Bat thì là Event của Animation CeilOut
    }

    protected override void HandleIfBossDie(object obj)
    {
        _hasGotHit = true;
        _notPlayDeadSfx = true;
        ChangeState(_mEnemiesGotHitState);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.collider.CompareTag(GameConstants.BULLET_TAG))
        {
            string bulletID = collision.collider.GetComponent<BulletController>().BulletID;
            EventsManager.NotifyObservers(EEvents.BulletOnHit, bulletID);
            ChangeState(_mEnemiesGotHitState);
        }
        else if (collision.collider.CompareTag(GameConstants.BOX_TAG) && _state is MEnemiesPatrolState)
            FlippingSprite();
        else if (collision.collider.CompareTag(GameConstants.TRAP_TAG) && !_hasGotHit && _state is not MEnemiesGotHitState)
        {
            _hasGotHit = true;
            ChangeState(_mEnemiesGotHitState);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConstants.PLAYER_TAG) && !_hasGotHit && _state is not MEnemiesGotHitState)
        {
            _hasGotHit = true;
            EventsManager.NotifyObservers(EEvents.PlayerOnJumpPassive);
            SpawnRewardForPlayer();
            SpawnDeathFX(EPoolable.EnemyDeathStarVfx);
            ChangeState(_mEnemiesGotHitState);
            SpawnBountyIfMarked();
        }
        else if (collision.CompareTag(GameConstants.TRAP_TAG) && !_hasGotHit && _state is not MEnemiesGotHitState
            || collision.CompareTag(GameConstants.DEAD_ZONE_TAG) && !_hasGotHit && _state is not MEnemiesGotHitState)
        {
            _hasGotHit = true;
            ChangeState(_mEnemiesGotHitState);
        }
    }

}
