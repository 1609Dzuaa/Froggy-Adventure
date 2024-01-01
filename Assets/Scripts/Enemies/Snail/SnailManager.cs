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
    //Khoảng thgian mình muốn ốc thực sự chui ra sau khi 0 detect Player ở gần
    [SerializeField] private float _idleDelay;

    //Rotate sprite after got hit
    [Header("Z Rotation When Dead")]
    [SerializeField] private float _degreeEachRotation;
    [SerializeField] private float _rotateTime;
    [SerializeField] private float _timeEachRotation;

    [Header("XY Offset")]
    [SerializeField] private float _xOffset;
    [SerializeField] private float _yOffset;

    private SnailIdleState _snailIdleState = new();
    private SnailPatrolState _snailPatrolState = new();
    private SnailAttackState _snailAttackState = new();
    private SnailShellHitState _snailShellHitState = new();
    private SnailGotHitState _snailGotHitState = new();
    private bool _hasDetectedGround;
    public bool _hasFlip;
    private float _entryTime;
    public bool _doneFlip;
    public bool _isMovingVertical;
    public int _direction = 1;

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
    }

    protected override void Update()
    {
        _state.Update();
        DetectedPlayer();
        DetectWall();
        DetectGround();
        DrawRayDetectPlayer();
        DrawRayDetectWall();
        DrawRayDetectGround();
        if (!_hasDetectedGround && !_hasFlip)
        {
            //Prob here dẫn đến Prob dưới
            _hasFlip = true;
            _doneFlip = false;
            _entryTime = Time.time;
            Debug.Log("0 thay ground");
        }
        HandleIfNotDetectedGround();
        //Debug.Log("Current Z Angles: " + currentZAngles);
        //transform.eulerAngles goes from 0-360
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
        if (Time.time - _entryTime >= _timeEachRotation && _hasFlip && !_doneFlip)
        {
            float currentZAngles = WrapAngle(transform.localEulerAngles.z);
            currentZAngles += _degreeEachRotation;

            if (!_isMovingVertical)
            {
                if (currentZAngles >= 90f)
                {
                    currentZAngles = 90f;
                    _direction = 2;
                    _doneFlip = true;
                    _isMovingVertical = true;
                    _hasFlip = false;
                    Debug.Log("Done Flip1, dir: " + _direction);
                    transform.position = new Vector3(transform.position.x - _xOffset, transform.position.y - _yOffset, transform.position.z);
                    /*if (!_isFacingRight)
                        transform.position = new Vector3(transform.position.x + _xOffset, transform.position.y - _yOffset, transform.position.z);
                    else
                        transform.position = new Vector3(transform.position.x - _xOffset, transform.position.y - _yOffset, transform.position.z);*/
                }
            }
            else
            {
                if (currentZAngles >= 180f)
                {
                    currentZAngles = 180f;
                    _direction = 3;
                    _doneFlip = true;
                    _isMovingVertical = false;
                    _hasFlip = false;
                    Debug.Log("Done Flip2, dir: " + _direction);
                    transform.position = new Vector3(transform.position.x + _xOffset, transform.position.y + _yOffset, transform.position.z);
                    /*if (!_isFacingRight)
                        transform.position = new Vector3(transform.position.x - _xOffset, transform.position.y, transform.position.z);
                    else
                        transform.position = new Vector3(transform.position.x + _xOffset, transform.position.y, transform.position.z);*/
                }
            }

            Debug.Log("Current Z Angles: " + currentZAngles);
            //currentZAngles += _degreeEachRotation;
            float currentYAngles = WrapAngle(transform.localEulerAngles.y);
            transform.rotation = Quaternion.Euler(0f, currentYAngles, currentZAngles);
            //transform.Rotate(0f, 0f, zDegree);
            //Debug.Log("Zdegree: " + zDegree);
            _entryTime = Time.time;
        }
    }

    protected override bool DetectedPlayer()
    {
        if (PlayerInvisibleBuff.Instance.IsAllowToUpdate)
            return _hasDetectedPlayer = false;

        if (!_isMovingVertical)
        {
            if (!_isFacingRight)
                _hasDetectedPlayer = Physics2D.Raycast(new Vector2(_playerCheck.position.x, _playerCheck.position.y), Vector2.left, _checkDistance, _playerLayer);
            else
                _hasDetectedPlayer = Physics2D.Raycast(new Vector2(_playerCheck.position.x, _playerCheck.position.y), Vector2.right, _checkDistance, _playerLayer);
        }
        else
        {
            if (!_isFacingRight)
                _hasDetectedPlayer = Physics2D.Raycast(new Vector2(_playerCheck.position.x, _playerCheck.position.y), Vector2.down, _checkDistance, _playerLayer);
            else
                _hasDetectedPlayer = Physics2D.Raycast(new Vector2(_playerCheck.position.x, _playerCheck.position.y), Vector2.up, _checkDistance, _playerLayer);
        }

        return _hasDetectedPlayer;
    }

    protected override void DrawRayDetectPlayer()
    {
        /*if (!_isMovingVertical)
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
        else
        {
            if (_hasDetectedPlayer)
            {
                if (!_isFacingRight)
                    Debug.DrawRay(_playerCheck.position, Vector2.down * _checkDistance, Color.red);
                else
                    Debug.DrawRay(_playerCheck.position, Vector2.up * _checkDistance, Color.red);
            }
            else
            {
                if (!_isFacingRight)
                    Debug.DrawRay(_playerCheck.position, Vector2.down * _checkDistance, Color.green);
                else
                    Debug.DrawRay(_playerCheck.position, Vector2.up * _checkDistance, Color.green);
            }
        }*/
    }

    protected override void DetectWall()
    {
        //Snail sẽ detect wall theo style của nó
        if(!_isMovingVertical)
        {
            if (!_isFacingRight)
                _hasCollidedWall = Physics2D.Raycast(new Vector2(_wallCheck.position.x, _wallCheck.position.y), Vector2.left, _wallCheckDistance, _wallLayer);
            else
                _hasCollidedWall = Physics2D.Raycast(new Vector2(_wallCheck.position.x, _wallCheck.position.y), Vector2.right, _wallCheckDistance, _wallLayer);
        }
        else
        {
            if (!_isFacingRight)
                _hasCollidedWall = Physics2D.Raycast(new Vector2(_wallCheck.position.x, _wallCheck.position.y), Vector2.down, _wallCheckDistance, _wallLayer);
            else
                _hasCollidedWall = Physics2D.Raycast(new Vector2(_wallCheck.position.x, _wallCheck.position.y), Vector2.up, _wallCheckDistance, _wallLayer);
        }
    }

    private void DrawRayDetectWall()
    {
        //Move vertical thì chỉnh left/right thành 2 vector down/up
        /*if (!_isMovingVertical)
        {
            if (!_isFacingRight)
                Debug.DrawRay(_wallCheck.position, Vector2.left, Color.green);
            else
                Debug.DrawRay(_wallCheck.position, Vector2.right, Color.green);
        }
        else
        {
            if (!_isFacingRight)
                Debug.DrawRay(_wallCheck.position, Vector2.down, Color.red);
            else
                Debug.DrawRay(_wallCheck.position, Vector2.up, Color.red);
        }*/
    }

    private void DetectGround()
    {
        if (!_isMovingVertical)
        {
            if (_direction == 1)
            {
                _hasDetectedGround = Physics2D.Raycast(_groundCheck.position, Vector2.down, _wallCheckDistance, _wallLayer);
                Debug.Log("detecG dir1");
            }
            else
            {
                _hasDetectedGround = Physics2D.Raycast(_groundCheck.position, Vector2.up, _wallCheckDistance, _wallLayer);
                Debug.Log("detecG dir3");
            }
        }
        else
        {
            if (_direction == 2)
            {
                _hasDetectedGround = Physics2D.Raycast(_groundCheck.position, Vector2.right, _wallCheckDistance, _wallLayer);
                Debug.Log("detecG dir2");
            }
            else
            {
                _hasDetectedGround = Physics2D.Raycast(_groundCheck.position, Vector2.left, _wallCheckDistance, _wallLayer);
                Debug.Log("detecG dir4");
            }
        }
        /*if (!_isMovingVertical)
            _hasDetectedGround = Physics2D.Raycast(_groundCheck.position, Vector2.down, _wallCheckDistance, _wallLayer);
        else
        {
            if (!_isFacingRight)
            {
                _hasDetectedGround = Physics2D.Raycast(_groundCheck.position, Vector2.right, _wallCheckDistance, _wallLayer);
                Debug.Log("Ground 1");
            }
            else
            {
                _hasDetectedGround = Physics2D.Raycast(_groundCheck.position, Vector2.left, _wallCheckDistance, _wallLayer);
                Debug.Log("Ground 2");

            }
        }*/
    }

    private void DrawRayDetectGround()
    {
        if (!_isMovingVertical)
        {
            if (_direction == 1)
                Debug.DrawRay(_groundCheck.position, Vector2.down, Color.green);
            else
                Debug.DrawRay(_groundCheck.position, Vector2.up, Color.green);
        }
        else
        {
            if (_direction == 2)
                Debug.DrawRay(_groundCheck.position, Vector2.right, Color.green);
            else
                Debug.DrawRay(_groundCheck.position, Vector2.left, Color.green);
        }
        /*if (!_isMovingVertical)
            Debug.DrawRay(_groundCheck.position, Vector2.down, Color.green);
        else
        {
            if (!_isFacingRight)
                Debug.DrawRay(_groundCheck.position, Vector2.right, Color.green);
            else
                Debug.DrawRay(_groundCheck.position, Vector2.left, Color.green);
        }*/
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
