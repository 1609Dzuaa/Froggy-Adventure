using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailManager : MEnemiesManager
{
    //Khi chui vào vỏ thì trâu hơn bthg x _healthPoint lần (Mặc định bthg đạp lên trên vỏ 1 lần là chết)
    //Nhìn chung tạm ổn, còn mỗi vấn đề snap con sên(cần tính dist từ nó tới ground để snap)
    //Sau khi thêm detect wall vẫn còn bug nhỏ rotate loạn xạ đoạn từ dir 2 -> 1 (wall)
    //Do việc snap dẫn đến snail có phần "lún" vào trong G
    //Nên Raycast 0 bắt đc collision
    //Stop the ray begins inside a collider
    //Docs, Ref:
    //https://docs.unity3d.com/ScriptReference/Physics-queriesHitBackfaces.html
    //“Note: This function will return false if you cast a ray
    //from inside a sphere to the outside; this in an intended behaviour.”
    //Solution: Kiểm tra những chỗ "snap" sao cho nó 0 khiến snail "lún" vào G,
    //hoặc vào Project Settings mục Physics2D chỉnh ở trong đó. 

    //Còn bug của snail khi bị hit ở rìa sẽ tự động rotate

    [Header("Health Point")]
    [SerializeField] private int _healthPoint;

    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckDistance;
    //private bool _hasDetectedGround;

    [Header("Detect Player2")]
    [Tooltip("Khoảng thgian sau khi ốc bị hit mà mình muốn nó chui ra sau khi 0 detect Player ở gần")]
    [SerializeField] private float _idleDelayAfterGotHit;

    //Rotate sprite after got hit
    [Header("Z Rotation When Dead")]
    [SerializeField] private float _degreeEachRotation;
    [SerializeField] private float _rotateTime;
    [SerializeField] private float _timeEachRotation;

    [Header("Delay Rotate")]
    [SerializeField] private float _delayRotate;

    [Header("Offset")]
    [SerializeField] private float _offSet;
    [Tooltip("Offset của box Trigger")]
    [SerializeField] private Vector2 _boxColTriggerOffset;

    [Header("Adjust Size Box Col")]
    [SerializeField] private Vector2 _adjustSizeBoxCol;

    private SnailIdleState _snailIdleState = new();
    private SnailPatrolState _snailPatrolState = new();
    private SnailAttackState _snailAttackState = new();
    private SnailShellHitState _snailShellHitState = new();
    private SnailGotHitState _snailGotHitState = new();
    private RaycastHit2D _hasDetectedGround;
    private BoxCollider2D _boxCol2D;
    private BoxCollider2D _boxCol2DTrigger;
    private BoxCollider2D[] _arrBoxCol2D = new BoxCollider2D[2];
    private Vector2 _originBoxSize;
    private Vector2 _originOffset;
    private bool _hasRotate;
    private float _entryTime;
    private bool _doneRotate;
    private bool _isMovingVertical;
    private int _direction = 1;
    private bool _hasStart;
    private bool _rotateByWall;
    //Solution cho việc nếu hit snail khi nó đang/chbi rotate => bug
    //Chỉ cho dmg nó nếu direction của nó là 1 (move ngang) - hạn chế bug
    private bool _isVunerable;
    private RaycastHit2D _groundHit;

    public bool HasRotate { get {  return _hasRotate; } }

    public bool DoneRotate { get { return _doneRotate; } }

    public bool IsMovingVertical { get { return _isMovingVertical; } }   

    public int Direction { get {  return _direction; } }    

    public bool HasDetectedGround { get { return _hasDetectedGround; } }

    public float HealthPoint { get { return _healthPoint; } set { _healthPoint = (int)value; } }

    public float DelayIdleAfterGotHit { get { return _idleDelayAfterGotHit; } }

    public float DegreeEachRotation { get { return _degreeEachRotation; } }

    public float TimeEachRotation { get { return _timeEachRotation; } }

    public SnailPatrolState SnailPatrolState { get { return _snailPatrolState; } }

    public SnailAttackState SnailAttackState { get { return _snailAttackState; } }

    public BoxCollider2D BoxCol2D { get => _boxCol2D; set => _boxCol2D = value; }

    public BoxCollider2D BoxCol2DTrigger { get => _boxCol2DTrigger; set => _boxCol2DTrigger = value; }

    public Vector2 AdjustBoxSize { get => _adjustSizeBoxCol; }

    public Vector2 OriginBoxSize { get => _originBoxSize; }

    public Vector2 OriginOffset { get => _originOffset; }

    public Vector2 OffsetBoxTrigger { get => _boxColTriggerOffset; }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void GetReferenceComponents()
    {
        base.GetReferenceComponents();
        _collider2D = GetComponent<Collider2D>();
        _arrBoxCol2D = GetComponents<BoxCollider2D>();
        foreach (var box in _arrBoxCol2D)
        {
            if (box.isTrigger)
                _boxCol2DTrigger = box;
            else
                _boxCol2D = box;
        }
        _originBoxSize = _boxCol2D.size;
        _originOffset = _boxCol2DTrigger.offset;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void SetUpProperties()
    {
        _state = _snailIdleState;
        _state.EnterState(this);
    }

    protected override void Update()
    {
        _state.Update();
        if (_state is SnailGotHitState)
            return;

        DetectedPlayer();
        DetectWall();
        DetectGround();
        Debug.Log("Dist: " + _groundHit.distance);
        DrawRayDetectPlayer();
        DrawRayDetectWall();
        DrawRayDetectGround();

        if (_hasCollidedWall && !_hasStart)
            StartCoroutine(StartTickWallRotation());
        else if (!_hasDetectedGround && !_hasStart)
            StartCoroutine(StartTickGroundRotation());

        if (_rotateByWall)
            RotateIfDetectedWall();
        else
            RotateIfNotDetectedGround();
        //Debug.Log("isVun: " + _isVunerable);
        //transform.eulerAngles goes from 0-360
    }

    private IEnumerator StartTickWallRotation()
    {
        _rotateByWall = true;
        _hasStart = true; //Lock, only start coroutine once if detected W/ not detected G
        _isVunerable = true;
        _hasRotate = true;
        _doneRotate = false;
        _entryTime = Time.time;
        //Debug.Log("dam wall");
        yield return null;
    }

    private IEnumerator StartTickGroundRotation()
    {
        _hasStart = true;
        _isVunerable = true;

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
        if (_rotateByWall)
        {
            //Wall thì ngược lại với Ground
            //Flip dần dần -90 độ theo trục z 

            if (Time.time - _entryTime >= _timeEachRotation && _hasRotate && !_doneRotate)
            {
                float currentZAngles = WrapAngle(transform.localEulerAngles.z);
                currentZAngles -= _degreeEachRotation;

                if (!_isMovingVertical)
                {
                    if (currentZAngles <= 90f && _direction == 3)
                    {
                        //Nếu đang direction 3 (góc Euler trục z lúc này = -180* = 180*)
                        //Thì theo nguyên tắc rotate khi đụng wall thì phải rotate trục z đến góc -270*
                        //Nhưng ngoài Inspector khi hiển thị sẽ chỉ chạy từ -180* -> 180*
                        //Nên thay vào đó rotate nó về góc 90* (90* = -270*)
                        //=> 180* trừ dần về 90*

                        currentZAngles = 90f;
                        _direction = 2;
                        _hasStart = false;
                        _doneRotate = true;
                        _isMovingVertical = true;
                        _hasRotate = false;
                        _rotateByWall = false;
                        transform.position = new Vector3(transform.position.x + _offSet, transform.position.y, transform.position.z);
                        //Debug.Log("Done RotateW3");
                    }
                    else if (currentZAngles <= -90f && _direction == 1)
                    {
                        //Nếu đang direction 1 thì rotate 1 góc 90 trục z theo chiều âm (=>hướng lên)
                        //và chuyển sang direction 4
                        currentZAngles = -90f;
                        _direction = 4;
                        _hasStart = false;
                        _doneRotate = true;
                        _isMovingVertical = true;
                        _hasRotate = false;
                        _rotateByWall = false;
                        //Debug.Log("Done RotateW1");
                        transform.position = new Vector3(transform.position.x - _offSet, transform.position.y, transform.position.z);
                    }
                }
                else
                {
                    //Nếu dir đang là 4 thì giới hạn góc Euler trục z ở -180* và switch dir sang 3
                    if (currentZAngles <= -180f)
                    {
                        currentZAngles = -180f;
                        _direction = 3;
                        _hasStart = false;
                        _doneRotate = true;
                        _isMovingVertical = false;
                        _hasRotate = false;
                        _rotateByWall = false;
                        //Debug.Log("Done RotateW4, deo snap Y");
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                        //Debug.Log("OffsetY cho 3: " + _hasDetectedGround.distance);
                    }
                    else if (currentZAngles <= 0f && _direction == 2)
                    {
                        //Nếu đang là dir 2 thì cố định góc Euler là 0* và sw dir sang 1
                        //90* -> 0* (90* = -270*)
                        currentZAngles = 0f;
                        _direction = 1;
                        _isVunerable = false;
                        _hasStart = false;
                        _doneRotate = true;
                        _isMovingVertical = false;
                        _hasRotate = false;
                        _rotateByWall = false;
                        //transform.position = new Vector3(transform.position.x, transform.position.y - _offSet, transform.position.z);
                        //Debug.Log("Done RotateW2, deo can snap");
                    }
                }

                //Debug.Log("Current Z Angles: " + currentZAngles);
                float currentYAngles = WrapAngle(transform.localEulerAngles.y);
                transform.rotation = Quaternion.Euler(0f, currentYAngles, currentZAngles);
                _entryTime = Time.time;
            }
        }
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
                    transform.position = new Vector3(transform.position.x, transform.position.y + _offSet, transform.position.z);
                    //Debug.Log("OffsetY cho 3: " + _hasDetectedGround.distance);
                }
                else if( currentZAngles >= 0f && _direction == 4)
                {
                    //Nếu đang là dir 4 thì cố định góc là 0 độ và chuyển dir sang 1
                    currentZAngles = 0f;
                    _direction = 1;
                    _isVunerable = false;
                    _hasStart = false;
                    _doneRotate = true;
                    _isMovingVertical = false;
                    _hasRotate = false;
                    transform.position = new Vector3(transform.position.x, transform.position.y - _offSet, transform.position.z);
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
        if (BuffsManager.Instance.GetTypeOfBuff(GameEnums.EBuffs.Invisible))
            return _hasDetectedPlayer = false;

        if (!_isMovingVertical)
        {
            if (!_isFacingRight)
                _hasDetectedPlayer = Physics2D.Raycast(new Vector2(_playerCheck.position.x, _playerCheck.position.y), Vector2.left, _enemiesSO.PlayerCheckDistance, _enemiesSO.PlayerLayer);
            else
                _hasDetectedPlayer = Physics2D.Raycast(new Vector2(_playerCheck.position.x, _playerCheck.position.y), Vector2.right, _enemiesSO.PlayerCheckDistance, _enemiesSO.PlayerLayer);
        }
        else
        {
            if (!_isFacingRight)
                _hasDetectedPlayer = Physics2D.Raycast(new Vector2(_playerCheck.position.x, _playerCheck.position.y), Vector2.down, _enemiesSO.PlayerCheckDistance, _enemiesSO.PlayerLayer);
            else
                _hasDetectedPlayer = Physics2D.Raycast(new Vector2(_playerCheck.position.x, _playerCheck.position.y), Vector2.up, _enemiesSO.PlayerCheckDistance, _enemiesSO.PlayerLayer);
        }

        return _hasDetectedPlayer;
    }

    protected override void DrawRayDetectPlayer()
    {
        if (!_isMovingVertical)
        {
            if (_hasDetectedPlayer)
            {
                if (_direction == 1)
                    Debug.DrawRay(_playerCheck.position, Vector2.left * _enemiesSO.PlayerCheckDistance, Color.red);
                else
                    Debug.DrawRay(_playerCheck.position, Vector2.right * _enemiesSO.PlayerCheckDistance, Color.red);
            }
            else
            {
                if (_direction == 1)
                    Debug.DrawRay(_playerCheck.position, Vector2.left * _enemiesSO.PlayerCheckDistance, Color.green);
                else
                    Debug.DrawRay(_playerCheck.position, Vector2.right * _enemiesSO.PlayerCheckDistance, Color.green);
            }
        }
        else
        {
            if (_hasDetectedPlayer)
            {
                if (_direction == 2)
                    Debug.DrawRay(_playerCheck.position, Vector2.down * _enemiesSO.PlayerCheckDistance, Color.red);
                else
                    Debug.DrawRay(_playerCheck.position, Vector2.up * _enemiesSO.PlayerCheckDistance, Color.red);
            }
            else
            {
                if (_direction == 2)
                    Debug.DrawRay(_playerCheck.position, Vector2.down * _enemiesSO.PlayerCheckDistance, Color.green);
                else
                    Debug.DrawRay(_playerCheck.position, Vector2.up * _enemiesSO.PlayerCheckDistance, Color.green);
            }
        }
    }

    protected override void DetectWall()
    {
        //Snail sẽ detect wall theo style của nó
        if (!_isMovingVertical)
        {
            if (_direction == 1)
                _hasCollidedWall = Physics2D.Raycast(_wallCheck.position, Vector2.left, _mEnemiesSO.WallCheckDistance, _mEnemiesSO.WallLayer);
            else
                _hasCollidedWall = Physics2D.Raycast(_wallCheck.position, Vector2.right, _mEnemiesSO.WallCheckDistance, _mEnemiesSO.WallLayer);
        }
        else
        {
            if (_direction == 2)
                _hasCollidedWall = Physics2D.Raycast(_wallCheck.position, Vector2.down, _mEnemiesSO.WallCheckDistance, _mEnemiesSO.WallLayer);
            else
                _hasCollidedWall = Physics2D.Raycast(_wallCheck.position, Vector2.up, _mEnemiesSO.WallCheckDistance, _mEnemiesSO.WallLayer);
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
                _hasDetectedGround = Physics2D.Raycast(_groundCheck.position, Vector2.down, _groundCheckDistance, _mEnemiesSO.WallLayer);
                _groundHit = Physics2D.Raycast(_groundCheck.position, Vector2.down, _groundCheckDistance, _mEnemiesSO.WallLayer);
            }
            else
            {
                _hasDetectedGround = Physics2D.Raycast(_groundCheck.position, Vector2.up, _groundCheckDistance, _mEnemiesSO.WallLayer);
                _groundHit= Physics2D.Raycast(_groundCheck.position, Vector2.up, _groundCheckDistance, _mEnemiesSO.WallLayer);
            }
        }
        else
        {
            if (_direction == 2)
            {
                _hasDetectedGround = Physics2D.Raycast(_groundCheck.position, Vector2.right, _groundCheckDistance, _mEnemiesSO.WallLayer);
                _groundHit= Physics2D.Raycast(_groundCheck.position, Vector2.right, _groundCheckDistance, _mEnemiesSO.WallLayer);
            }
            else
            {
                _hasDetectedGround = Physics2D.Raycast(_groundCheck.position, Vector2.left, _groundCheckDistance, _mEnemiesSO.WallLayer);
                _groundHit= Physics2D.Raycast(_groundCheck.position, Vector2.left, _groundCheckDistance, _mEnemiesSO.WallLayer);
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
    }

    private void ChangeToAttack()
    {
        ChangeState(_snailAttackState);
        _hasGotHit = false;
        //Event của animation Shell_Hit
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConstants.PLAYER_TAG) && !_hasGotHit)
        {
            _hasGotHit = true;
            var playerScript = collision.GetComponent<PlayerStateManager>();
            playerScript.GetRigidBody2D().AddForce(playerScript.GetPlayerStats.JumpOnEnemiesForce, ForceMode2D.Impulse);
            if (_healthPoint == 0)
                ChangeState(_snailGotHitState);
            else
            {
                CancelInvoke();
                if (!_isVunerable)
                    ChangeState(_snailShellHitState);
            }
        }
    }

    protected override void AllowAttackPlayer()
    {
        if (BuffsManager.Instance.GetTypeOfBuff(GameEnums.EBuffs.Invisible))
        {
            ChangeState(_snailIdleState);
            return;
        }

        ChangeState(_snailAttackState);
    }
}
