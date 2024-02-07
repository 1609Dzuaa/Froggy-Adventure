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

    [Header("Speed"), Range(0,100)]
    [SerializeField] private float _movementSpeed;

    [Header("Time")]
    [SerializeField] private float _timeEachScale;

    [Header("Constant")]
    [SerializeField] private float _scaleFactorEachDeltaTime; //Muốn tăng bao nhiêu mỗi delta time

    [Header("Is Vertical")]
    [SerializeField] private bool _isVertical;

    [Header("Movement Direction")]
    [SerializeField] private GameEnums.ERockMove _moveDir;

    private Rigidbody2D _rb;
    private float _entryTime; //bộ đếm thgian
    private float _scaleFactor; //hệ số scale speed
    private float _tempSpeed; //biến speed để set trong FixedUpdate
    private bool _isHitLeft;
    private bool _isHitRight;
    private bool _isHitTop;
    private bool _isHitBottom;

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
    }

    protected override void SetUpProperties()
    {
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
        if (!_isVertical)
        {
            switch (_moveDir)
            {
                case GameEnums.ERockMove.Left:
                    _rb.velocity = new Vector2(-_tempSpeed, 0f);
                    break;
                case GameEnums.ERockMove.Right:
                    _rb.velocity = new Vector2(_tempSpeed, 0f);
                    break;
                case GameEnums.ERockMove.Top:
                    _rb.velocity = new Vector2(0f, _tempSpeed);
                    break;
                case GameEnums.ERockMove.Bottom:
                    _rb.velocity = new Vector2(0f, -_tempSpeed);
                    break;
            }
        }
        else
        {
            switch (_moveDir)
            {
                case GameEnums.ERockMove.Top:
                    _rb.velocity = new Vector2(0f, _tempSpeed);
                    break;
                case GameEnums.ERockMove.Bottom:
                    _rb.velocity = new Vector2(0f, -_tempSpeed);
                    break;
            }
        }
    }

    private void WallCheck()
    {
        switch (_moveDir)
        {
            case GameEnums.ERockMove.Left:
                _isHitLeft = Physics2D.Raycast(_leftCheck.position, Vector2.left, _checkDistance, _wallLayer);
                break;
            case GameEnums.ERockMove.Right:
                _isHitRight = Physics2D.Raycast(_rightCheck.position, Vector2.right, _checkDistance, _wallLayer);
                break;
            case GameEnums.ERockMove.Top:
                _isHitTop = Physics2D.Raycast(_topCheck.position, Vector2.up, _checkDistance, _wallLayer);
                break;
            case GameEnums.ERockMove.Bottom:
                _isHitBottom = Physics2D.Raycast(_bottomCheck.position, Vector2.down, _checkDistance, _wallLayer);
                break;
        }
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
        if(!_isVertical)
        {
            if (_isHitLeft && _moveDir != GameEnums.ERockMove.Top)
            {
                _anim.SetTrigger("Left");
                _isHitLeft = false;
                _moveDir = GameEnums.ERockMove.Top;
                _entryTime = Time.time;
                _scaleFactor = 0;
                
            }
            else if (_isHitRight && _moveDir != GameEnums.ERockMove.Bottom)
            {
                _anim.SetTrigger("Right");
                _isHitRight = false;
                _moveDir = GameEnums.ERockMove.Bottom;
                _entryTime = Time.time;
                _scaleFactor = 0;   
            }
            else if (_isHitTop && _moveDir != GameEnums.ERockMove.Right)
            {
                _anim.SetTrigger("Top");
                _isHitTop = false;
                _moveDir = GameEnums.ERockMove.Right;
                _entryTime = Time.time;
                _scaleFactor = 0;                
            }
            else if (_isHitBottom && _moveDir != GameEnums.ERockMove.Left)
            {
                _anim.SetTrigger("Bot");
                _isHitBottom = false;
                _moveDir = GameEnums.ERockMove.Left;
                _entryTime = Time.time;
                _scaleFactor = 0;              
            }
        }
        else
        {
            if (_isHitBottom && _moveDir != GameEnums.ERockMove.Top)
            {
                _anim.SetTrigger("Bot");
                _isHitBottom = false;
                _moveDir = GameEnums.ERockMove.Top;
                _entryTime = Time.time;
                _scaleFactor = 0;               
            }
            else if (_isHitTop && _moveDir != GameEnums.ERockMove.Bottom)
            {
                _anim.SetTrigger("Top");
                _isHitTop = false;
                _moveDir = GameEnums.ERockMove.Bottom;
                _entryTime = Time.time;
                _scaleFactor = 0;               
            }
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
