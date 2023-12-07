using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailManager : MEnemiesManager
{
    //Kill this enemy x times to unlock WallSlide Skill
    //Nên check dựa trên Distance chứ 0 phải ray

    [Header("Rotate Related")]
    [SerializeField] private float _dtEachRotation;
    [SerializeField] private float _degreeEachRotation;

    private SnailIdleState _snailIdleState = new();
    private SnailPatrolState _snailPatrolState = new();
    private SnailAttackState _snailAttackState = new();

    public SnailIdleState SnailIdleState { get { return _snailIdleState; } }

    public SnailPatrolState SnailPatrolState { get { return _snailPatrolState; } }

    public SnailAttackState SnailAttackState { get { return _snailAttackState; } }

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
        base.Update();
        DrawRayDetectWall();
        /*if (_hasCollidedWall && !_hasFlip)
        {
            _hasFlip = true;
            /*_entryTime = Time.time;
            if (Time.time - _entryTime >= _dtEachRotation)
            {
                if (transform.eulerAngles.z <= 270.0f && !_isFacingRight
                    )//|| transform.eulerAngles.z >= 90.0f && _isFacingRight)
                {
                    if (!_hasFlip)
                    {
                        _hasFlip = true;
                        transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + _degreeEachRotation);
                    }
                    return;
                }else 
                transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + _degreeEachRotation);
                _entryTime = Time.time;
            }
            transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + _degreeEachRotation);
            _snailPatrolState.CanMoveVertical = true;
        }*/
        /*if(Time.time - _entryTime >= _dtEachRotation)
        {
            if (transform.eulerAngles.z <= 270.0f && !_isFacingRight
                )//|| transform.eulerAngles.z >= 90.0f && _isFacingRight)
                return;
            transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + _degreeEachRotation);
            _entryTime = Time.time;
        }*/
        //Debug.Log("Can MV: " + _snailPatrolState.CanMoveVertical);
        //Debug.Log("Z, isFR: " + transform.eulerAngles.z + ", " + _isFacingRight);
    }


    protected override void DetectWall()
    {
        //base.DetectWall();
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

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override void AllowAttackPlayer()
    {
        ChangeState(_snailAttackState);
    }
}
