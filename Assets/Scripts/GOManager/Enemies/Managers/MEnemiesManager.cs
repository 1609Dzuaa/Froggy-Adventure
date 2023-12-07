using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MEnemiesManager : EnemiesManager
{
    //MEnemies <-> MoveableEnemies
    private MEnemiesIdleState _mEnemiesIdleState = new();
    private MEnemiesPatrolState _mEnemiesPatrolState = new();
    private MEnemiesAttackState _mEnemiesAttackState = new();
    private MEnemiesGotHitState _mEnemiesGotHitState = new();

    [Header("Boundaries")]
    [SerializeField] protected Transform _boundaryLeft;
    [SerializeField] protected Transform _boundaryRight;

    [Header("Wall Check")]
    [SerializeField] protected Transform _wallCheck;
    [SerializeField] protected float _wallCheckDistance;
    [SerializeField] protected LayerMask _wallLayer;
    protected bool _hasCollidedWall;

    [Header("Time")]
    [SerializeField] protected float _restTime;
    [SerializeField] protected float _patrolTime;
    [SerializeField] protected float _attackDelay;

    [Header("Speed")]
    [SerializeField] protected float _patrolSpeed;
    [SerializeField] protected float _chaseSpeed;

    [Header("Parent")]
    [SerializeField] protected GameObject _parent;
    protected Collider2D _collider;

    //Public Field

    public MEnemiesIdleState MEnemiesIdleState { get { return _mEnemiesIdleState; } }
    
    public MEnemiesPatrolState MEnemiesPatrolState { get { return _mEnemiesPatrolState;} set { _mEnemiesPatrolState = value; } }

    public MEnemiesAttackState MEnemiesAttackState { get { return _mEnemiesAttackState; } set { _mEnemiesAttackState = value; } }

    public MEnemiesGotHitState MEnemiesGotHitState { get { return _mEnemiesGotHitState; } set { _mEnemiesGotHitState = value; } }

    public float GetRestTime() { return this._restTime; }

    public float GetPatrolTime() { return this._patrolTime; }

    public float GetAttackDelay() { return this._attackDelay; }

    public bool HasCollidedWall { get { return this._hasCollidedWall; } }

    public float GetPatrolSpeed() { return this._patrolSpeed; }

    public float GetChaseSpeed() { return this._chaseSpeed; }

    public Collider2D Collider2D { get { return _collider; } }

    public Transform BoundaryLeft { get { return _boundaryLeft; } }

    public Transform BoundaryRight { get { return _boundaryRight; } }

    public void SetHasGotHit(bool para) { this._hasGotHit = para; }

    //Các lớp sau muốn mở rộng hàm Start, Update,... thì nhớ gọi base.Start(),... trong hàm Start của chính nó
    //Còn 0 implement gì thêm thì 0 cần làm gì, nó tự động đc gọi trong đây ngay cả khi là private
    protected override void Start()
    {
        base.Start(); //Lấy anim, rb từ EnemiesManager
        _state = _mEnemiesIdleState;
        _state.EnterState(this);
        _collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        _state.Update();
        DetectPlayer();
        DetectWall();
    }

    protected virtual void FixedUpdate()
    {
        _state.FixedUpdate();
        //Debug.Log("Called");
    }


    //Call this in Patrol State
    public virtual void Move(float velo)
    {
        if (_isFacingRight)
            this._rb.velocity = new Vector2(velo, _rb.velocity.y);
        else
            this._rb.velocity = new Vector2(-velo, _rb.velocity.y);
    }

    protected virtual void DetectPlayer()
    {
        if (!_isFacingRight)
            _hasDetectedPlayer = Physics2D.Raycast(new Vector2(_playerCheck.position.x, _playerCheck.position.y), Vector2.left, _checkDistance, _playerLayer);
        else
            _hasDetectedPlayer = Physics2D.Raycast(new Vector2(_playerCheck.position.x, _playerCheck.position.y), Vector2.right, _checkDistance, _playerLayer);

        DrawRayDetectPlayer();
    }

    protected virtual void DetectWall()
    {
        if (!_isFacingRight)
            _hasCollidedWall = Physics2D.Raycast(new Vector2(_wallCheck.position.x, _wallCheck.position.y), Vector2.left, _wallCheckDistance, _wallLayer);
        else
            _hasCollidedWall = Physics2D.Raycast(new Vector2(_wallCheck.position.x, _wallCheck.position.y), Vector2.right, _wallCheckDistance, _wallLayer);
    }

    protected virtual void DrawRayDetectPlayer()
    {
        if (_hasDetectedPlayer)
        {
            if (!_isFacingRight)
                Debug.DrawRay(_playerCheck.position, Vector2.left * _checkDistance, Color.red);
            else
                Debug.DrawRay(_playerCheck.position, Vector2.right * _checkDistance, Color.red);
        }
        else
        {
            if (!_isFacingRight)
                Debug.DrawRay(_playerCheck.position, Vector2.left * _checkDistance, Color.green);
            else
                Debug.DrawRay(_playerCheck.position, Vector2.right * _checkDistance, Color.green);
        }
    }

    //Hàm này dùng để Invoke khi detect ra Player
    protected virtual void AllowAttackPlayer()
    {
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

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player" && !_hasGotHit)
        {
            _hasGotHit = true;
            var playerScript = collision.GetComponent<PlayerStateManager>();
            playerScript.GetRigidBody2D().AddForce(playerScript.GetJumpOnEnemiesForce(), ForceMode2D.Impulse);
            ChangeState(_mEnemiesGotHitState);
        }
    }

    //Event của Got Hit Animation
    protected void DestroyParent()
    {
        Destroy(_parent);
    }

}
