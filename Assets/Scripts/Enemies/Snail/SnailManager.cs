using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailManager : MEnemiesManager
{
    //bug vai loz nên quyết định Player đạp phát chết luôn con sên
    //còn việc snap cho nó

    [Header("Z Rotation When Move")]
    [SerializeField] private float _degreeEachRotation;
    [SerializeField] private float _rotateTime;
    [SerializeField] private float _timeEachRotation;

    [Header("Delay Rotate")]
    [SerializeField] private float _delayRotate;

    [Header("Offset")]
    [SerializeField] private float _offSet;

    private SnailPatrolState _snailPatrolState = new();
    private SnailGotHitState _snailGotHitState = new();
    private bool _hasRotate;
    private float _entryTime;
    private bool _doneRotate;
    private bool _isMovingVertical;
    private int _direction = 1;
    private bool _hasStart;
    private bool _rotateByWall;
    private BoxCollider2D _boxCol;
    //Solution cho việc nếu hit snail khi nó đang/chbi rotate => bug
    //Chỉ cho dmg nó nếu direction của nó là 1 (move ngang) - hạn chế bug
    private RaycastHit2D _groundHit;

    public bool HasRotate { get {  return _hasRotate; } }

    public bool DoneRotate { get { return _doneRotate; } }

    public bool IsMovingVertical { get { return _isMovingVertical; } }   

    public int Direction { get {  return _direction; } }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void GetReferenceComponents()
    {
        base.GetReferenceComponents();
        _collider2D = GetComponent<Collider2D>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void SetUpProperties()
    {
        base.SetUpProperties();
        _state = _snailPatrolState;
        _state.EnterState(this);
    }

    protected override void Update()
    {
        base.Update();
        if (_state is SnailGotHitState)
            return;

        DrawRayDetectPlayer();
        DrawRayDetectWall();

        if (_hasCollidedWall && !_hasStart)
            StartCoroutine(StartTickWallRotation());
        else if (!_hasDetectedGround && !_hasStart)
            StartCoroutine(StartTickGroundRotation());

        if (_rotateByWall)
            RotateIfDetectedWall();
        else
            RotateIfNotDetectedGround();
        //transform.eulerAngles goes from 0-360
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConstants.PLAYER_TAG) && !_hasGotHit)
        {
            StopAllCoroutines();
            EventsManager.Instance.NotifyObservers(GameEnums.EEvents.PlayerOnJumpPassive, null);
            _hasGotHit = true;
            ChangeState(_snailGotHitState);
        }
    }

    private IEnumerator StartTickWallRotation()
    {
        _rotateByWall = true;
        _hasStart = true; //Lock, only start coroutine once if detected W/ not detected G
        _hasRotate = true;
        _doneRotate = false;
        _entryTime = Time.time;
        //Debug.Log("dam wall");
        yield return null;
    }

    private IEnumerator StartTickGroundRotation()
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
                        transform.position = new Vector3(transform.position.x + _groundHit.distance, transform.position.y, transform.position.z);
                        //transform.position = new Vector3(transform.position.x + _offSet, transform.position.y, transform.position.z);
                        Debug.Log("Done RotateW3: " + _groundHit.distance);
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
                        transform.position = new Vector3(transform.position.x - _groundHit.distance, transform.position.y, transform.position.z);
                        Debug.Log("Done RotateW1: " + _groundHit.distance);
                        //transform.position = new Vector3(transform.position.x - _offSet, transform.position.y, transform.position.z);
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
                        Debug.Log("Done RotateW4, deo snap Y");
                        //Debug.Log("OffsetY cho 3: " + _hasDetectedGround.distance);
                    }
                    else if (currentZAngles <= 0f && _direction == 2)
                    {
                        //Nếu đang là dir 2 thì cố định góc Euler là 0* và sw dir sang 1
                        //90* -> 0* (90* = -270*)
                        currentZAngles = 0f;
                        _direction = 1;
                        _hasStart = false;
                        _doneRotate = true;
                        _isMovingVertical = false;
                        _hasRotate = false;
                        _rotateByWall = false;
                        Debug.Log("Done RotateW2, deo can snap");
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
                    //Debug.Log("Done RotateG4, deo snap");
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
                    //Debug.Log("Done RotateG2, deo snap");
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
                    //transform.position = new Vector3(transform.position.x, transform.position.y + _groundHit.distance, transform.position.z);
                    //Debug.Log("Done RotateG3: " + _groundHit.point);
                    transform.position = new Vector3(transform.position.x, transform.position.y + _offSet, transform.position.z);
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
                    //transform.position = new Vector3(transform.position.x, transform.position.y - _groundHit.distance, transform.position.z);
                    //Debug.Log("Done RotateG1: " + _groundHit.point);
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

    protected override bool DetectWall()
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

        return _hasCollidedWall;
    }

    protected override void DrawRayDetectWall()
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

    protected override void DetectGround()
    {
        //Move vertical thì chỉnh left/right thành 2 vector down/up
        if (!_isMovingVertical)
        {
            if (_direction == 1)
            {
                _groundHit = Physics2D.Raycast(_groundCheck.position, Vector2.down, _mEnemiesSO.GroundCheckDistance, _mEnemiesSO.WallLayer);
                _hasDetectedGround = Physics2D.Raycast(_groundCheck.position, Vector2.down, _mEnemiesSO.GroundCheckDistance, _mEnemiesSO.WallLayer);
                _groundHit = Physics2D.Raycast(_groundCheck.position, Vector2.down, _mEnemiesSO.GroundCheckDistance, _mEnemiesSO.WallLayer);
            }
            else
            {
                _groundHit = Physics2D.Raycast(_groundCheck.position, Vector2.up, _mEnemiesSO.GroundCheckDistance, _mEnemiesSO.WallLayer);
                _hasDetectedGround = Physics2D.Raycast(_groundCheck.position, Vector2.up, _mEnemiesSO.GroundCheckDistance, _mEnemiesSO.WallLayer);
                _groundHit= Physics2D.Raycast(_groundCheck.position, Vector2.up, _mEnemiesSO.GroundCheckDistance, _mEnemiesSO.WallLayer);
            }
        }
        else
        {
            if (_direction == 2)
            {
                _groundHit = Physics2D.Raycast(_groundCheck.position, Vector2.right, _mEnemiesSO.GroundCheckDistance, _mEnemiesSO.WallLayer);
                _hasDetectedGround = Physics2D.Raycast(_groundCheck.position, Vector2.right, _mEnemiesSO.GroundCheckDistance, _mEnemiesSO.WallLayer);
                _groundHit= Physics2D.Raycast(_groundCheck.position, Vector2.right, _mEnemiesSO.GroundCheckDistance, _mEnemiesSO.WallLayer);
            }
            else
            {
                _groundHit = Physics2D.Raycast(_groundCheck.position, Vector2.left, _mEnemiesSO.GroundCheckDistance, _mEnemiesSO.WallLayer);
                _hasDetectedGround = Physics2D.Raycast(_groundCheck.position, Vector2.left, _mEnemiesSO.GroundCheckDistance, _mEnemiesSO.WallLayer);
                _groundHit= Physics2D.Raycast(_groundCheck.position, Vector2.left, _mEnemiesSO.GroundCheckDistance, _mEnemiesSO.WallLayer);
            }
        }
    }

    protected override void DrawRayDetectGround()
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

    /*protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.collider.CompareTag(GameConstants.GROUND_TAG))
            Debug.Log("dist: " + collision.collider.Distance(_collider2D).pointA);
    }*/
}
