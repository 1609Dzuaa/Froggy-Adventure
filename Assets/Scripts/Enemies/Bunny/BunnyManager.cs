using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyManager : MEnemiesManager
{
    //Bật cao như Bunny => Cân nhắc việc design kill 1 lượng enemies Bunny thì unlock DbJump
    //Vẫn còn bug flip sprite lung tung khi attack @@
    
    [Header("PlayerRef")]
    [SerializeField] private Transform _playerRef;

    [Header("JumpHeight")]
    [SerializeField] private float _jumpHeight;

    private BunnyIdleState _bunnyIdleState = new();
    private BunnyPatrolState _bunnyPatrolState = new();
    private BunnyAttackJumpState _bunnyAtkJumpState = new();
    private BunnyAttackFallState _bunnyAtkFallState = new();
    private BunnyGotHitState _bunnyGotHitState = new();
    private bool _isPlayerBackward;

    public Transform PlayerRef { get { return _playerRef; } }

    public BunnyIdleState BunnyIdleState { get { return this._bunnyIdleState; } }

    public BunnyPatrolState BunnyPatrolState { get { return this._bunnyPatrolState; } }

    public BunnyAttackJumpState BunnyAttackJumpState { get { return this._bunnyAtkJumpState; } }

    public BunnyAttackFallState BunnyAttackFallState { get { return this._bunnyAtkFallState; } }

    public bool IsPlayerBackWard { get { return this._isPlayerBackward; } }

    public float JumpHeight { get { return this._jumpHeight; } }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        _state = _bunnyIdleState;
        _state.EnterState(this);
        MEnemiesGotHitState = _bunnyGotHitState;
    }

    protected override void Update()
    {
        base.Update();
        BackwardCheck();
        DrawBackwardCheckRay();
        //Debug.Log("IPBw: " + _isPlayerBackward);
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
        if (BuffsManager.Instance.GetTypeOfBuff(GameEnums.EBuffs.Invisible))
            return;

        ChangeState(_bunnyAtkJumpState);
    }

    protected override bool DetectedPlayer()
    {
        return base.DetectedPlayer();
    }

    private void BackwardCheck()
    {
        if (!_isFacingRight)
            _isPlayerBackward = Physics2D.Raycast(new Vector2(_playerCheck.position.x, _playerCheck.position.y), Vector2.right, _checkDistance, _playerLayer);
        else
            _isPlayerBackward = Physics2D.Raycast(new Vector2(_playerCheck.position.x, _playerCheck.position.y), Vector2.left, _checkDistance, _playerLayer);
    }

    private void DrawBackwardCheckRay()
    {
        if (_isPlayerBackward)
        {
            if (!_isFacingRight)
                Debug.DrawRay(_playerCheck.position, Vector2.right * _checkDistance, Color.yellow);
            else
                Debug.DrawRay(_playerCheck.position, Vector2.left * _checkDistance, Color.yellow);
        }
        else
        {
            if (!_isFacingRight)
                Debug.DrawRay(_playerCheck.position, Vector2.right * _checkDistance, Color.blue);
            else
                Debug.DrawRay(_playerCheck.position, Vector2.left * _checkDistance, Color.blue);
        }
    }
}
