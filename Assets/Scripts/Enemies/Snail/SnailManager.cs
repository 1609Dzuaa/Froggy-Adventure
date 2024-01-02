using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailManager : MEnemiesManager
{
    //Khi chui vào vỏ thì trâu hơn bthg x _healthPoint lần (Mặc định bthg đạp lên trên vỏ 1 lần là chết)
    //Nhìn chung tạm ổn, còn mỗi vấn đề snap con sên(cần tính dist từ nó tới ground để snap)

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

    [Header("Delay Rotate")]
    [SerializeField] private float _delayRotate;

    [Header("Y Offset")]
    [SerializeField] private float _yOffset;

    private SnailIdleState _snailIdleState = new();
    private SnailPatrolState _snailPatrolState = new();
    private SnailAttackState _snailAttackState = new();
    private SnailShellHitState _snailShellHitState = new();
    private SnailGotHitState _snailGotHitState = new();
    private bool _hasDetectWall;
    private RaycastHit2D _hasDetectedGround;
    private bool _hasRotate;
    private float _entryTime;
    private bool _doneRotate;
    private bool _isMovingVertical;
    private int _direction = 1;
    private bool _hasStart;

    public bool HasRotate { get {  return _hasRotate; } }

    public bool DoneRotate { get { return _doneRotate; } }

    public bool IsMovingVertical { get { return _isMovingVertical; } }   

    public int Direction { get {  return _direction; } }    

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
        if (!_hasDetectedGround && !_hasStart)
            StartCoroutine(StartTickRotation());

        RotateIfNotDetectedGround();
        //Debug.Log("Current Z Angles: " +transform.localEulerAngles.z);
        //transform.eulerAngles goes from 0-360
    }

    private IEnumerator StartTickRotation()
    {
        _hasStart = true;

        yield return new WaitForSeconds(_delayRotate);

        _hasRotate = true;
        _doneRotate = false;
        _entryTime = Time.time;
        //Debug.Log("0 thay ground");
    }

    private float WrapAngle(float angle)
    {
        angle %= 360;
        if (angle > 180)
            return angle - 360;

        return angle;
        //Func này để trả về góc giữa -180 -> 180 Euler Angles
        //Mục đích đọc Euler Angles ngoài Inspector
    }

    private void RotateIfDetectedWall()
    {
        //Còn việc xử lý đâm wall của con vk này
    }

    private void RotateIfNotDetectedGround()
    {
        //Cách rotate:
        //Xác định direction(dir): 
        //2<---1
        //|    |
        //3--->4
        //Từ 1->2->3 xoay 0 -> 90 -> 180 trục z tăng dần
        //Từ 3->4->1 xoay từ -180 -> -90 -> 0 trục z tăng dần
        //Vì khi đạt ngưỡng 180 độ thì ngoài Inspector nó chuyển về -180 ? :Đ

        if (Time.time - _entryTime >= _timeEachRotation && _hasRotate && !_doneRotate)
        {
            float currentZAngles = WrapAngle(transform.localEulerAngles.z);
            currentZAngles += _degreeEachRotation;

            if (!_isMovingVertical)
            {
                //Nếu đang là dir 3 thì cố định góc là -90 độ và chuyển dir sang 4
                //Tại sao là -90 độ mà 0 phải 270 độ ?
                //Ngoài Inspector nó chỉ chạy từ -180 đến 180 độ
                //=> 270 độ <=> -90 độ
                if (currentZAngles >= -90f && currentZAngles < 0f && _direction == 3)
                {
                    currentZAngles = -90f;
                    _direction = 4;
                    _hasStart = false;
                    _doneRotate = true;
                    _isMovingVertical = true;
                    _hasRotate = false;
                    //Debug.Log("Done Rotate4, currAZ: " + _direction);
                }
                else if (currentZAngles >= 90f && _direction == 1)
                {
                    //Nếu đang là dir 1 thì cố định góc là 90 độ và chuyển dir sang 2
                    currentZAngles = 90f;
                    _direction = 2;
                    _hasStart = false;
                    _doneRotate = true;
                    _isMovingVertical = true;
                    _hasRotate = false;
                } 
            }
            else
            {
                //Nếu đang là dir 2 thì cố định góc là 180 độ và chuyển dir sang 3
                if (currentZAngles >= 180f)
                {
                    currentZAngles = 180f;
                    _direction = 3;
                    _hasStart = false;
                    _doneRotate = true;
                    _isMovingVertical = false;
                    _hasRotate = false;                    
                    transform.position = new Vector3(transform.position.x, transform.position.y + _yOffset, transform.position.z);
                    //Debug.Log("OffsetY cho 3: " + _hasDetectedGround.distance);
                }
                else if( currentZAngles >= 0f && _direction == 4)
                {
                    //Nếu đang là dir 4 thì cố định góc là 0 độ và chuyển dir sang 1
                    currentZAngles = 0f;
                    _direction = 1;
                    _hasStart = false;
                    _doneRotate = true;
                    _isMovingVertical = false;
                    _hasRotate = false;
                    transform.position = new Vector3(transform.position.x, transform.position.y - _yOffset, transform.position.z);
                    //Debug.Log("OffsetY cho 1: " + _hasDetectedGround.distance);
                }
            }

            //Debug.Log("Current Z Angles: " + currentZAngles);
            float currentYAngles = WrapAngle(transform.localEulerAngles.y);
            transform.rotation = Quaternion.Euler(0f, currentYAngles, currentZAngles);
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
        if (!_isMovingVertical)
        {
            if (_direction == 1)
                _hasCollidedWall = Physics2D.Raycast(_wallCheck.position, Vector2.left, _wallCheckDistance, _wallLayer);
            else
                _hasCollidedWall = Physics2D.Raycast(_wallCheck.position, Vector2.right, _wallCheckDistance, _wallLayer);
        }
        else
        {
            if (_direction == 2)
                _hasCollidedWall = Physics2D.Raycast(_wallCheck.position, Vector2.down, _wallCheckDistance, _wallLayer);
            else
                _hasCollidedWall = Physics2D.Raycast(_wallCheck.position, Vector2.up, _wallCheckDistance, _wallLayer);
        }
    }

    private void DrawRayDetectWall()
    {
        if (!_isMovingVertical)
        {
            if (_hasCollidedWall)
            {
                if (_direction == 1)
                    Debug.DrawRay(_wallCheck.position, Vector2.left, Color.red);
                else
                    Debug.DrawRay(_wallCheck.position, Vector2.right, Color.red);
            }
            else
            {
                if (_direction == 1)
                    Debug.DrawRay(_wallCheck.position, Vector2.left, Color.green);
                else
                    Debug.DrawRay(_wallCheck.position, Vector2.right, Color.green);
            }
        }
        else
        {
            if (_hasCollidedWall)
            {
                if (_direction == 2)
                    Debug.DrawRay(_wallCheck.position, Vector2.down, Color.red);
                else
                    Debug.DrawRay(_wallCheck.position, Vector2.up, Color.red);
            }
            else
            {
                if (_direction == 2)
                    Debug.DrawRay(_wallCheck.position, Vector2.down, Color.green);
                else
                    Debug.DrawRay(_wallCheck.position, Vector2.up, Color.green);
            }
        }
    }

    private void DetectGround()
    {
        //Move vertical thì chỉnh left/right thành 2 vector down/up
        if (!_isMovingVertical)
        {
            if (_direction == 1)
            {
                _hasDetectedGround = Physics2D.Raycast(_groundCheck.position, Vector2.down, _wallCheckDistance, _wallLayer);
                //Debug.Log("detecG dir1");
            }
            else
            {
                _hasDetectedGround = Physics2D.Raycast(_groundCheck.position, Vector2.up, _wallCheckDistance, _wallLayer);
                //Debug.Log("detecG dir3");
            }
        }
        else
        {
            if (_direction == 2)
            {
                _hasDetectedGround = Physics2D.Raycast(_groundCheck.position, Vector2.right, _wallCheckDistance, _wallLayer);
                //Debug.Log("detecG dir2");
            }
            else
            {
                _hasDetectedGround = Physics2D.Raycast(_groundCheck.position, Vector2.left, _wallCheckDistance, _wallLayer);
                //Debug.Log("detecG dir4");
            }
        }
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
        /*else if (collision.name == "Bound1")
        {
            if (!_hasRotate)
            {
                _hasRotate = true;
                _doneRotate = false;
                _entryTime = Time.time;
                Debug.Log("B1");
            }
        }
        else if(collision.name == "Bound2")
        {
            if(!_hasRotate)
            {
                _hasRotate = true;
                _doneRotate = false;
                _entryTime = Time.time;
                Debug.Log("B2");
            }
        }
        else if (collision.name == "Bound3")
        {
            if (!_hasRotate)
            {
                _hasRotate = true;
                _doneRotate = false;
                _entryTime = Time.time;
                Debug.Log("B3");
            }
        }
        else if (collision.name == "Bound4")
        {
            if (!_hasRotate)
            {
                _hasRotate = true;
                _doneRotate = false;
                _entryTime = Time.time;
                Debug.Log("B4");
            }
        }*/
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
