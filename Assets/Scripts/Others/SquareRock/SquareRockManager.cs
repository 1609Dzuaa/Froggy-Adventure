using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareRockManager : GameObjectManager
{
    //SqrRock sẽ di chuyển chậm lúc mới hitwall, sau đó nhanh lên
    //Nó 0 dmg Player nhưng nếu Player bị ép vào wall thì sẽ bị chịu dmg
    //Lý giải việc RbBodyType là Kinematic nhưng 0 va chạm với TilemapCollider2D:
    //https://forum.unity.com/threads/tilemap-collider-not-working-with-kinematic-object.1000029/
    //=>Solution: giữ nguyên RbBodyType là Dynamic và set grav scale về 0

    [Header("Boundaries Check")]
    [SerializeField] private Transform _leftCheck;
    [SerializeField] private Transform _rightCheck;
    [SerializeField] private Transform _topCheck;
    [SerializeField] private Transform _bottomCheck;

    [Header("Check Field")]
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private float _checkDistance;

    [Header("Speed")]
    [SerializeField] private float _movementSpeed;

    [Header("Time")]
    [SerializeField] private float _timeEachScale;

    [Header("Constant")]
    [SerializeField] private float _scaleFactorEachDeltaTime; //Muốn tăng bao nhiêu mỗi delta time

    private Rigidbody2D _rb;
    private float _entryTime; //bộ đếm thgian
    private float _scaleFactor; //hệ số scale speed
    private float _tempSpeed; //biến speed để set trong FixedUpdate
    private bool _isHitLeft;
    private bool _isHitRight;
    private bool _isHitTop;
    private bool _isHitBottom;
    private bool _isMovingLeft;
    private bool _isMovingRight;
    private bool _isMovingTop;
    private bool _isMovingBottom;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void GetReferenceComponents()
    {
        base.GetReferenceComponents();
        _rb = GetComponent<Rigidbody2D>();
    }

    protected override void Start()
    {
        base.Start();
        SetUpProperties();
    }

    protected override void SetUpProperties()
    {
        _isMovingLeft = true;
        _entryTime = Time.time;
        _scaleFactor = 0;
        _rb.gravityScale = 0f; //Tránh nếu bị set nhầm ngoài Inspector
    }

    private void Update()
   {
        WallCheck();
        HandleScaleSpeed();
        DrawRay();
        HandleAnimationAndLogic();
        //Debug.Log("IsMV L, R, T, B: " + _isMovingLeft + ", " + _isMovingRight + ", " + _isMovingTop + ", " + _isMovingBottom);
    }

    private void FixedUpdate()
    {
            if (_isMovingLeft)
                _rb.velocity = new Vector2(-_tempSpeed, 0f);
            else if (_isMovingRight)
                _rb.velocity = new Vector2(_tempSpeed, 0f);
            else if (_isMovingTop)
                _rb.velocity = new Vector2(0f, _tempSpeed);
            else if (_isMovingBottom)
                _rb.velocity = new Vector2(0f, -_tempSpeed);
        //Debug.Log("velo: " + _rb.velocity);
    }

    private void WallCheck()
    {
        if (_isMovingLeft)
            _isHitLeft = Physics2D.Raycast(_leftCheck.position, Vector2.left, _checkDistance, _wallLayer);
        else if (_isMovingRight)
            _isHitRight = Physics2D.Raycast(_rightCheck.position, Vector2.right, _checkDistance, _wallLayer);
        else if (_isMovingTop)
            _isHitTop = Physics2D.Raycast(_topCheck.position, Vector2.up, _checkDistance, _wallLayer);
        else if (_isMovingBottom)
            _isHitBottom = Physics2D.Raycast(_bottomCheck.position, Vector2.down, _checkDistance, _wallLayer);
    }

    private void DrawRay()
    {
        Debug.DrawRay(_leftCheck.position, Vector2.left * _checkDistance, Color.green);
        Debug.DrawRay(_rightCheck.position, Vector2.right * _checkDistance, Color.green);
        Debug.DrawRay(_topCheck.position, Vector2.up * _checkDistance, Color.green);
        Debug.DrawRay(_bottomCheck.position, Vector2.down * _checkDistance, Color.green);
    }

    private void HandleAnimationAndLogic()
    {
        if (_isHitLeft && !_isMovingTop)
        {
            _anim.SetTrigger("Left");
            _isHitLeft = false;
            _isMovingTop = true;
            _isMovingLeft = _isMovingRight = _isMovingBottom = false;
            _entryTime = Time.time;
            _scaleFactor = 0;
        }
        else if (_isHitRight && !_isMovingBottom)
        {
            _anim.SetTrigger("Right");
            _isHitRight = false;
            _isMovingBottom = true;
            _isMovingLeft = _isMovingRight = _isMovingTop = false;
            _entryTime = Time.time;
            _scaleFactor = 0;
        }
        else if (_isHitTop && !_isMovingRight)
        {
            _anim.SetTrigger("Top");
            _isHitTop = false;
            _isMovingRight = true;
            _isMovingLeft = _isMovingTop = _isMovingBottom = false;
            _entryTime = Time.time;
            _scaleFactor = 0;
        }
        else if (_isHitBottom && !_isMovingLeft)
        {
            _anim.SetTrigger("Bot");
            _isHitBottom = false;
            _isMovingLeft = true;
            _isMovingTop = _isMovingRight = _isMovingBottom = false;
            _entryTime = Time.time;
            _scaleFactor = 0;
        }
    }

    private void HandleScaleSpeed()
    {
        if (Time.time - _entryTime >= _timeEachScale)
        {
            _entryTime = Time.time;
            _scaleFactor += _scaleFactorEachDeltaTime;
        }
        if (_scaleFactor > 1.0f)
            _scaleFactor = 1.0f;
        _tempSpeed = _movementSpeed * _scaleFactor;
        //Scale vận tốc theo delta time thay vì set 1 phát luôn như cũ
    }

    private void BackToBlinkAnimation()
    {
        _anim.SetTrigger("Blink");
        //Event của LTRB Hit animation
    }

}
