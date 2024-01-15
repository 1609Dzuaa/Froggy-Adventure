﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeManager : MEnemiesManager
{
    //Xài prefab Bee thì chỉ định 3 phần min/max + Nest
    //Only Chase & Attack Player if he comes too close to the Nest
    [Header("Nest")]
    [SerializeField] private Transform _beeNest;

    [Header("Boundaries")]
    [SerializeField] private Transform _boundaryLeft;
    [SerializeField] private Transform _boundaryRight;

    [Header("Weapon Field")]
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _shootPosition;

    [Header("Range")]
    [SerializeField] private float _triggerAttackRange;
    [SerializeField] private float _adjustYAxisAttackRange;

    [Header("YOffset")]
    [SerializeField] private float _yOffset;

    [Header("Attack Pos Radius")]
    [SerializeField] private float _atkPosRad;

    private BeeIdleState _beeIdleState = new();
    private BeePatrolState _beePatrolState = new();
    private BeeChaseState _beeChaseState = new();
    private BeeAttackState _beeAttackState = new();
    private BeeGotHitState _beeGotHitState = new();
    private bool _mustAttack;

    public float AdJustYAxisAttackRange() { return _adjustYAxisAttackRange; }

    public float YOffSet { get { return _yOffset; } }

    public Transform BoundaryLeft { get { return _boundaryLeft; } }

    public Transform BoundaryRight { get { return _boundaryRight; } }

    public BeeIdleState GetBeeIdleState() { return _beeIdleState; }

    public BeePatrolState GetBeePatrolState() { return _beePatrolState; }

    public BeeChaseState GetBeeChaseState() { return _beeChaseState; }

    public BeeAttackState GetBeeAttackState() { return _beeAttackState; }

    public Transform GetPlayer() { return _playerCheck; }

    public bool MustAttack { get { return _mustAttack; } set { _mustAttack = value; } }

    public void SetIsFacingRight(bool para) { _isFacingRight = para; }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void GetReferenceComponents()
    {
        base.GetReferenceComponents();
        _playerCheck = FindObjectOfType<PlayerStateManager>().transform;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void SetUpProperties()
    {
        _mEnemiesIdleState = _beeIdleState;
        _mEnemiesPatrolState = _beePatrolState;
        _mEnemiesGotHitState = _beeGotHitState;
        base.SetUpProperties();
    }

    protected override void Update()
    {
        _state.Update();
        DetectedPlayer();
        //Debug.Log("IFR: " + _isFacingRight);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override bool DetectedPlayer()
    {
        if (BuffsManager.Instance.GetTypeOfBuff(GameEnums.EBuffs.Invisible).IsAllowToUpdate)
            return _hasDetectedPlayer = false;

        return _hasDetectedPlayer = Vector2.Distance(_playerCheck.position, _beeNest.position) <= _triggerAttackRange;
    }

    private void OnDrawGizmos()
    {
        if (_playerCheck)
        {
            Vector2 _attackPos = new Vector2(_playerCheck.position.x, _playerCheck.position.y + _adjustYAxisAttackRange);
            Gizmos.DrawSphere(_attackPos, _atkPosRad);
        }
    }

    public void SpawnBullet()
    {
        if (BuffsManager.Instance.GetTypeOfBuff(GameEnums.EBuffs.Invisible).IsAllowToUpdate)
        {
            ChangeState(_beeIdleState);
            _mustAttack = false;
            return;
        }

        GameObject bullet = BulletPool.Instance.GetObjectInPool(GameConstants.BEE_BULLET);
        bullet.SetActive(true);
        string bulletID = bullet.GetComponent<BulletController>().BulletID;

        BulletInfor info = new(GameConstants.BEE_BULLET, bulletID, _isFacingRight, _shootPosition.position);
        EventsManager.Instance.NotifyObservers(GameEnums.EEvents.BulletOnReceiveInfo, info);
    }

    public void AllowUpdateAttackState()
    {
        _beeAttackState.SetAllowUpdate(true);
        //Delay việc update ở state shoot nếu 0 sẽ bị loạn state
    }
}
