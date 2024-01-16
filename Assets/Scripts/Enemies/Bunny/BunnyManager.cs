using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyManager : MEnemiesManager
{
    //Vẫn còn bug flip sprite lung tung khi attack @@ - con
    [Header("JumpHeight")]
    [SerializeField] private float _jumpHeight;

    private Transform _playerRef;
    private BunnyIdleState _bunnyIdleState = new();
    private BunnyPatrolState _bunnyPatrolState = new();
    private BunnyAttackJumpState _bunnyAtkJumpState = new();
    private BunnyAttackFallState _bunnyAtkFallState = new();
    private BunnyGotHitState _bunnyGotHitState = new();
    private bool _isPlayerBackward;

    public Transform PlayerRef { get { return _playerRef; } }

    public BunnyIdleState BunnyIdleState { get { return _bunnyIdleState; } }

    public BunnyPatrolState BunnyPatrolState { get { return _bunnyPatrolState; } }

    public BunnyAttackFallState BunnyAttackFallState { get { return _bunnyAtkFallState; } }

    public bool IsPlayerBackWard { get { return _isPlayerBackward; } }

    public float JumpHeight { get { return _jumpHeight; } }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void GetReferenceComponents()
    {
        base.GetReferenceComponents();
        _playerRef = FindObjectOfType<PlayerStateManager>().transform;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void SetUpProperties()
    {
        _mEnemiesIdleState = _bunnyIdleState;
        _mEnemiesGotHitState = _bunnyGotHitState;
        base.SetUpProperties();
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

    protected override void AllowAttackPlayer()
    {
        //coi lai
        if (BuffsManager.Instance.GetTypeOfBuff(GameEnums.EBuffs.Invisible).IsAllowToUpdate)
            return;

        ChangeState(_bunnyAtkJumpState);
    }

    private void BackwardCheck()
    {
        if (BuffsManager.Instance.GetTypeOfBuff(GameEnums.EBuffs.Invisible).IsAllowToUpdate)
        {
            _isPlayerBackward = false;
            return;
        }

        if (!_isFacingRight)
            _isPlayerBackward = Physics2D.Raycast(new Vector2(_playerCheck.position.x, _playerCheck.position.y), Vector2.right, _enemiesSO.PlayerCheckDistance, _enemiesSO.PlayerLayer);
        else
            _isPlayerBackward = Physics2D.Raycast(new Vector2(_playerCheck.position.x, _playerCheck.position.y), Vector2.left, _enemiesSO.PlayerCheckDistance, _enemiesSO.PlayerLayer);
    }

    private void DrawBackwardCheckRay()
    {
        if (_isPlayerBackward)
        {
            if (!_isFacingRight)
                Debug.DrawRay(_playerCheck.position, Vector2.right * _enemiesSO.PlayerCheckDistance, Color.yellow);
            else
                Debug.DrawRay(_playerCheck.position, Vector2.left * _enemiesSO.PlayerCheckDistance, Color.yellow);
        }
        else
        {
            if (!_isFacingRight)
                Debug.DrawRay(_playerCheck.position, Vector2.right * _enemiesSO.PlayerCheckDistance, Color.blue);
            else
                Debug.DrawRay(_playerCheck.position, Vector2.left * _enemiesSO.PlayerCheckDistance, Color.blue);
        }
    }
}
