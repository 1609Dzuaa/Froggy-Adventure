using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailManager : MEnemiesManager
{
    //Kill this enemy x times to unlock WallSlide Skill
    //Khi chui vào vỏ thì trâu hơn bthg x _healthPoint lần (Mặc định bthg đạp lên trên vỏ 1 lần là chết)
    [Header("Health Point")]
    [SerializeField] private int _healthPoint;

    [Header("Detect Player2")]
    //Khoảng cách mình muốn nó detect Player
    [SerializeField] private float _detectRange;
    //Khoảng thgian mình muốn ốc thực sự chui ra sau khi 0 detect Player ở gần
    [SerializeField] private float _delayIdleTime;

    //Rotate sprite after got hit
    [Header("Z Rotation When Dead")]
    [SerializeField] private float _degreeEachRotation;
    [SerializeField] private float _timeEachRotation;

    private SnailIdleState _snailIdleState = new();
    private SnailPatrolState _snailPatrolState = new();
    private SnailAttackState _snailAttackState = new();
    private SnailShellHitState _snailShellHitState = new();
    private SnailGotHitState _snailGotHitState = new();
    private float _distanceToPlayer;

    public float HealthPoint { get { return _healthPoint; } set { _healthPoint = (int)value; } }

    public float DelayIdleTime { get { return _delayIdleTime; } }

    public float DegreeEachRotation { get { return _degreeEachRotation; } }

    public float TimeEachRotation { get { return _timeEachRotation; } }

    public SnailIdleState SnailIdleState { get { return _snailIdleState; } }

    public SnailPatrolState SnailPatrolState { get { return _snailPatrolState; } }

    public SnailAttackState SnailAttackState { get { return _snailAttackState; } }

    public SnailShellHitState SnailShellHitState { get { return _snailShellHitState; } }

    public SnailGotHitState SnailGotHitState { get { return _snailGotHitState; } }

    protected override void Start()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _state = _snailIdleState;
        _state.EnterState(this);
        _collider = GetComponent<Collider2D>();
    }

    protected override void Update()
    {
        _state.Update();
        DetectPlayer();
        DetectWall();
        DrawRayDetectPlayer();
        DrawRayDetectWall();
        //Debug.Log("HP: " + _healthPoint);
        //Coi lại min/max boundaries cho sên :v
    }

    protected override void DetectPlayer()
    {
        _distanceToPlayer = Vector2.Distance(transform.position, _playerCheck.position);
        if (_distanceToPlayer <= _detectRange)
            _hasDetectedPlayer = true;
        else
            _hasDetectedPlayer = false;
    }

    protected override void DrawRayDetectPlayer()
    {
        //DrawLine != DrawRay:
        //DrawLine vẽ từ đầu đến ĐÍCH chỉ định, trong khi
        //DrawRay vẽ 1 tia từ đầu đến hướng chỉ định
        if (_hasDetectedPlayer)
            Debug.DrawLine(transform.position, _playerCheck.transform.position, Color.red);
        else
            Debug.DrawLine(transform.position, _playerCheck.transform.position, Color.green);
    }

    protected override void DetectWall()
    {
        //Snail sẽ detect wall theo style của nó
        if(!_snailPatrolState.CanMoveVertical)
        {
            if (!_isFacingRight)
                _hasCollidedWall = Physics2D.Raycast(new Vector2(_wallCheck.position.x, _wallCheck.position.y), Vector2.left, _wallCheckDistance, _wallLayer);
            else
                _hasCollidedWall = Physics2D.Raycast(new Vector2(_wallCheck.position.x, _wallCheck.position.y), Vector2.right, _wallCheckDistance, _wallLayer);
        }
        else
        {
            if (!_isFacingRight)
                _hasCollidedWall = Physics2D.Raycast(new Vector2(_wallCheck.position.x, _wallCheck.position.y), Vector2.up, _wallCheckDistance, _wallLayer);
            else
                _hasCollidedWall = Physics2D.Raycast(new Vector2(_wallCheck.position.x, _wallCheck.position.y), Vector2.down, _wallCheckDistance, _wallLayer);
        }
    }

    protected override void ChangeToIdle()
    {
        ChangeState(_snailIdleState);
        _hasGotHit = false;
    }

    private void DrawRayDetectWall()
    {
        //Move vertical thì chỉnh left/right thành 2 vector up/down
        if (_snailPatrolState.CanMoveVertical)
        {
            if (!_isFacingRight)
                Debug.DrawRay(_wallCheck.position, Vector2.up * _wallCheckDistance, Color.red);
            else
                Debug.DrawRay(_wallCheck.position, Vector2.down * _wallCheckDistance, Color.red);
        }
        else
        {
            if (!_isFacingRight)
                Debug.DrawRay(_wallCheck.position, Vector2.left * _wallCheckDistance, Color.green);
            else
                Debug.DrawRay(_wallCheck.position, Vector2.right * _wallCheckDistance, Color.green);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player" && !_hasGotHit)
        {
            _hasGotHit = true;
            var playerScript = collision.GetComponent<PlayerStateManager>();
            playerScript.GetRigidBody2D().AddForce(playerScript.GetJumpOnEnemiesForce(), ForceMode2D.Impulse);
            if (_healthPoint == 0)
                ChangeState(_snailGotHitState);
            else
                ChangeState(_snailShellHitState);
        }
    }

    protected override void AllowAttackPlayer()
    {
        ChangeState(_snailAttackState);
    }
}
