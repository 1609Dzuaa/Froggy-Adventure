using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareRockManager : GameObjectManager
{
    //SqrRock sẽ di chuyển chậm lúc mới hitwall, sau đó nhanh lên
    //Nó 0 dmg Player nhưng nếu Player bị ép vào wall thì sẽ bị chịu dmg
    //Có nên cho v nhanh dần theo delta time ?

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
    [SerializeField] private float _delayBackToBlink;
    [SerializeField] private float _delayMoveFast;

    [Header("Constant")]
    [SerializeField] private float _lowSpeedConstant;

    private Rigidbody2D _rb;
    private bool _isHitLeft;
    private bool _isHitRight;
    private bool _isHitTop;
    private bool _isHitBottom;
    private bool _isMovingLeft;
    private bool _isMovingRight;
    private bool _isMovingTop;
    private bool _isMovingBottom;
    private bool _allowMoveFast;

    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody2D>();
    }

    protected override void Start()
    {
        base.Start();
        _isMovingLeft = true;
        StartCoroutine(AllowMoveFast());
    }

   private void Update()
   {
        WallCheck();
        DrawRay();
        HandleAnimation();
        //Debug.Log("IsMV L, R, T, B: " + _isMovingLeft + ", " + _isMovingRight + ", " + _isMovingTop + ", " + _isMovingBottom);
   }

    private void FixedUpdate()
    {
        float tempSpeed;
        if (!_allowMoveFast)
            tempSpeed = _movementSpeed * _lowSpeedConstant;
        else
            tempSpeed = _movementSpeed;

        if (_isMovingLeft)
            _rb.velocity = new Vector2(-tempSpeed, 0f);
        else if (_isMovingRight)
            _rb.velocity = new Vector2(tempSpeed, 0f);
        else if (_isMovingTop)
            _rb.velocity = new Vector2(0f, tempSpeed);
        else if (_isMovingBottom)
            _rb.velocity = new Vector2(0f, -tempSpeed);
        Debug.Log("velo: " + _rb.velocity);
    }

    private void WallCheck()
    {
        if (_isMovingLeft)
            _isHitLeft = Physics2D.Raycast(_leftCheck.position, Vector2.left, _checkDistance, _wallLayer);
        else if (_isMovingRight)
            _isHitRight = Physics2D.Raycast(_rightCheck.position, Vector2.right, _checkDistance, _wallLayer);
        else if (_isMovingTop)
        {
            _isHitTop = Physics2D.Raycast(_topCheck.position, Vector2.up, _checkDistance, _wallLayer);
            //Debug.Log("Here, hitTop, hitLeft: " + _isHitTop + ", " + _isHitLeft);
        }
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

    private void HandleAnimation()
    {
        if (_isHitLeft && !_isMovingTop)
        {
            _anim.SetTrigger("Left");
            _isHitLeft = false;
            _isMovingTop = true;
            _isMovingLeft = _isMovingRight = _isMovingBottom = false;
            _allowMoveFast = false;
            StartCoroutine(AllowMoveFast());
            //3 thang con lai false
        }
        else if (_isHitRight && !_isMovingBottom)
        {
            _anim.SetTrigger("Right");
            _isHitRight = false;
            _isMovingBottom = true;
            _isMovingLeft = _isMovingRight = _isMovingTop = false;
            _allowMoveFast = false;
            StartCoroutine(AllowMoveFast());
        }
        else if (_isHitTop && !_isMovingRight)
        {
            _anim.SetTrigger("Top");
            _isHitTop = false;
            _isMovingRight = true;
            _isMovingLeft = _isMovingTop = _isMovingBottom = false;
            _allowMoveFast = false;
            StartCoroutine(AllowMoveFast());
        }
        else if (_isHitBottom && !_isMovingLeft)
        {
            _anim.SetTrigger("Bot");
            _isHitBottom = false;
            _isMovingLeft = true;
            _isMovingTop = _isMovingRight = _isMovingBottom = false;
            _allowMoveFast = false;
            StartCoroutine(AllowMoveFast());
        }
    }

    private IEnumerator BackToBlinkAnimation()
    {
        yield return new WaitForSeconds(_delayBackToBlink);

        _anim.SetTrigger("Blink");
        //Event của LTRB Hit animation
    }

    private IEnumerator AllowMoveFast()
    {
        yield return new WaitForSeconds(_delayMoveFast);

        _allowMoveFast = true;
    }

}
