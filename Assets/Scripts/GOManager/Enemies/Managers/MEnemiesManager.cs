using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MEnemiesManager : EnemiesManager, IMoveable
{
    public List<MEnemiesBaseState> ListState = new();
    private MEnemiesIdleState _mEnemiesIdleState = new();
    public MEnemiesPatrolState _mEnemiesPatrolState = new();
    public MEnemiesAttackState _mEnemiesAttackState = new();
    public MEnemiesGotHitState _mEnemiesGotHitState = new();

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
    [SerializeField] protected float _chaseDelayTime;

    [Header("Speed")]
    [SerializeField] protected Vector2 _patrolSpeed;
    [SerializeField] protected Vector2 _chaseSpeed;

    [Header("Parent")]
    [SerializeField] protected GameObject _parent;
    private Collider2D _collider;

    //Public Field

    public MEnemiesIdleState MEnemiesIdleState { get { return _mEnemiesIdleState; } }
    
    public MEnemiesPatrolState MEnemiesPatrolState { get { return _mEnemiesPatrolState;} }

    public MEnemiesAttackState MEnemiesAttackState { get { return _mEnemiesAttackState;} }

    public MEnemiesGotHitState MEnemiesGotHitState { get { return _mEnemiesGotHitState; } }

    public float GetRestTime() { return this._restTime; }

    public float GetPatrolTime() { return this._patrolTime; }

    public float GetChaseDelayTime() { return this._chaseDelayTime; }

    public bool HasCollidedWall { get { return this._hasCollidedWall; } }

    public Vector2 GetPatrolSpeed() { return this._patrolSpeed; }

    public Vector2 GetChaseSpeed() { return this._chaseSpeed; }

    public Collider2D Collider2D { get { return _collider; } }

    private void Awake()
    {
        var myEnumMemberCount = Enum.GetNames(typeof(EnumState.EMEnemiesState)).Length;

        for (int i = 0; i < myEnumMemberCount; i++)
            ListState.Add(_mEnemiesIdleState); 

    }

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
    public void Move(Vector2 velo)
    {
        if (_isFacingRight)
            this._rb.velocity = velo;
        else
            this._rb.velocity = new Vector2(-velo.x, velo.y);
    }

    private void DetectPlayer()
    {
        if (!_isFacingRight)
            _hasDetectedPlayer = Physics2D.Raycast(new Vector2(_playerCheck.position.x, _playerCheck.position.y), Vector2.left, _checkDistance, _playerLayer);
        else
            _hasDetectedPlayer = Physics2D.Raycast(new Vector2(_playerCheck.position.x, _playerCheck.position.y), Vector2.right, _checkDistance, _playerLayer);

        DrawRayDetectPlayer();
    }

    private void DetectWall()
    {
        if (!_isFacingRight)
            _hasCollidedWall = Physics2D.Raycast(new Vector2(_wallCheck.position.x, _wallCheck.position.y), Vector2.left, _wallCheckDistance, _wallLayer);
        else
            _hasCollidedWall = Physics2D.Raycast(new Vector2(_wallCheck.position.x, _wallCheck.position.y), Vector2.right, _wallCheckDistance, _wallLayer);
    }

    private void DrawRayDetectPlayer()
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

    //Event của Idle & Patrol Animation
    private void AllowAttack()
    {
        if (_state is MEnemiesIdleState)
            _mEnemiesIdleState.AllowAttack();
        else if (_state is MEnemiesPatrolState)
            _mEnemiesPatrolState.AllowAttack();
        //Delay 1 khoảng ngắn sau khi vào state để
        //tránh tình trạng quay mặt rồi attack ngay lập tức!
    }

    //Dùng để Invoke trong state Attack nếu 0 thấy player
    private void ChangeToIdle()
    {
        ChangeState(_mEnemiesIdleState);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player" && !_hasGotHit)
        {
            _hasGotHit = true;
            var playerScript = collision.GetComponent<PlayerStateManager>();
            playerScript.GetRigidBody2D().AddForce(playerScript.GetJumpOnEnemiesForce());
            ChangeState(_mEnemiesGotHitState);
        }
    }

    //Event của Got Hit Animation
    protected void DestroyParent()
    {
        Destroy(_parent);
    }

}
