using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailManager : MEnemiesManager
{
    //Khi chui vào vỏ thì trâu hơn bthg x _healthPoint lần (Mặc định bthg đạp lên trên vỏ 1 lần là chết)
    //Docs:
    //When you read the .eulerAngles property,
    //Unity converts the Quaternion's internal representation of the rotation
    //to Euler angles. Because, there is more than one way
    //to represent any given rotation using Euler angles,
    //the values you read back out may be quite DIFFERENT
    //from the values you assigned.
    //This can cause confusion if you are trying
    //to gradually increment the values to produce animation.


    [Header("Health Point")]
    [SerializeField] private int _healthPoint;

    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheck;
    //private bool _hasDetectedGround;

    [Header("Detect Player2")]
    //Khoảng cách mình muốn nó detect Player
    [SerializeField] private float _detectRange;
    //Khoảng thgian mình muốn ốc thực sự chui ra sau khi 0 detect Player ở gần
    [SerializeField] private float _idleDelay;

    //Rotate sprite after got hit
    [Header("Z Rotation When Dead")]
    [SerializeField] private float _degreeEachRotation;
    [SerializeField] private float _rotateTime;
    [SerializeField] private float _timeEachRotation;

    private SnailIdleState _snailIdleState = new();
    private SnailPatrolState _snailPatrolState = new();
    private SnailAttackState _snailAttackState = new();
    private SnailShellHitState _snailShellHitState = new();
    private SnailGotHitState _snailGotHitState = new();
    private float _distanceToPlayer;
    private bool _hasDetectedGround;
    public bool _hasFlip;
    private float _entryTime;
    private float zDegree = 0f;
    public bool _doneFlip;

    public bool HasDetectedGround { get { return _hasDetectedGround; } }

    public float HealthPoint { get { return _healthPoint; } set { _healthPoint = (int)value; } }

    public float DelayIdleTime { get { return _idleDelay; } }

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
        _collider2D = GetComponent<Collider2D>();
        //transform.Rotate(0f, 0f, 90f);
    }

    protected override void Update()
    {
        _state.Update();
        DetectedPlayer();
        DetectWall();
        DetectGround();
        DrawRayDetectPlayer();
        DrawRayDetectWall();
        if (!_hasDetectedGround && !_hasFlip)
        {
            _hasFlip = true;
            _entryTime = Time.time;
        }
        HandleIfNotDetectedGround();
        //if (Mathf.Abs(WrapAngle(transform.eulerAngles.z)) == 90f)
        //Debug.Log("enough");
        //transform.eulerAngles goes from 0-360
        //Debug.Log("X, Z degree: " + WrapAngle(transform.eulerAngles.x) + ", " + WrapAngle(transform.eulerAngles.z));
        //Debug.Log("Ground: " + _hasDetectedGround);
        Debug.Log("v: " + _rb.velocity);
    }

    private float WrapAngle(float angle)
    {
        angle %= 360;
        if (angle > 180)
            return angle - 360;

        return angle;
    }

    private void HandleIfNotDetectedGround()
    {
        if(Time.time - _entryTime >= _timeEachRotation && _hasFlip)
        {
            if (zDegree >= 90f)
            {
                _doneFlip = true;
                return;
            }

            //_rb.velocity = Vector2.zero;
            zDegree += _degreeEachRotation;
            transform.rotation = Quaternion.Euler(0f, 0f, zDegree);
            //transform.Rotate(0f, 0f, zDegree);
            //Debug.Log("Zdegree: " + zDegree);
            _entryTime = Time.time;
        }
    }

    protected override bool DetectedPlayer()
    {
        if (PlayerInvisibleBuff.Instance.IsAllowToUpdate)
            return _hasDetectedPlayer = false;

        _distanceToPlayer = Vector2.Distance(transform.position, _playerCheck.position);
        if (_distanceToPlayer <= _detectRange)
            _hasDetectedPlayer = true;
        else
            _hasDetectedPlayer = false;
        return _hasDetectedPlayer;
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

    private void DetectGround()
    {
        //_hasDetectedGround = Physics2D.Raycast(new Vector2(_groundCheck.position.x, _groundCheck.position.y), Vector2.down, _wallCheckDistance, _wallLayer);
        //Move vertical thì chỉnh left/right thành 2 vector up/down
        if(!_snailPatrolState.CanMoveVertical)
        {
            //if (!_isFacingRight)
                _hasDetectedGround = Physics2D.Raycast(new Vector2(_groundCheck.position.x, _groundCheck.position.y), Vector2.down, _wallCheckDistance, _wallLayer);
            Debug.DrawRay(_groundCheck.position, Vector2.down * _wallCheckDistance, Color.red);

            //else
            //_hasDetectedGround = Physics2D.Raycast(new Vector2(_groundCheck.position.x, _groundCheck.position.y), Vector2.right, _wallCheckDistance, _wallLayer);
        }
        else
        {
            //if (!_isFacingRight)
                _hasDetectedGround = Physics2D.Raycast(new Vector2(_groundCheck.position.x, _groundCheck.position.y), Vector2.right, _wallCheckDistance, _wallLayer);
            Debug.DrawRay(_groundCheck.position, Vector2.right * _wallCheckDistance, Color.red);

            //else
            //_hasDetectedGround = Physics2D.Raycast(new Vector2(_groundCheck.position.x, _groundCheck.position.y), Vector2.down, _wallCheckDistance, _wallLayer);
        }
    }

    protected override void ChangeToIdle()
    {
        ChangeState(_snailIdleState);
        _hasGotHit = false;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == GameConstants.PLAYER_NAME && !_hasGotHit)
        {
            _hasGotHit = true;
            var playerScript = collision.GetComponent<PlayerStateManager>();
            playerScript.GetRigidBody2D().AddForce(playerScript.GetPlayerStats.JumpOnEnemiesForce, ForceMode2D.Impulse);
            if (_healthPoint == 0)
                ChangeState(_snailGotHitState);
            else
                ChangeState(_snailShellHitState);
        }
    }

    protected override void AllowAttackPlayer()
    {
        if (PlayerInvisibleBuff.Instance.IsAllowToUpdate)
        {
            ChangeState(_snailIdleState);
            return;
        }

        ChangeState(_snailAttackState);
    }
}
