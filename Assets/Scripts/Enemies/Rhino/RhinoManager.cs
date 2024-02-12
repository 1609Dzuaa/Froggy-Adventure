﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhinoManager : MEnemiesManager
{
    private RhinoAttackState _rhinoAttackState = new();
    private RhinoWallHitState _rhinoWallHitState = new();

    [Tooltip("Mở rộng từ phần Time ở class MEnemiesManager")]
    [SerializeField] protected float _restDelay;

    [Header("KnockBackForce"), Tooltip("Lực áp vào khi HitShield")]
    [SerializeField] private Vector2 _knockBackForce;

    private bool _isHitShield;

    public bool IsHitShield { get { return _isHitShield; } set { _isHitShield = value; } }

    public RhinoWallHitState RhinoWallHitState { get { return _rhinoWallHitState; } }

    public float RestDelay { get { return _restDelay; } }

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void SetUpProperties()
    {
        _mEnemiesAttackState = _rhinoAttackState;
        base.SetUpProperties();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        //Debug.Log("Has dt G: " + _hasDetectedGround);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        HandleIfCollidedWithShield(collision);
    }

    private void HandleIfCollidedWithShield(Collision2D collision)
    {
        //Chỉ khi attack và va phải Shield thì mới change sang WH
        if (collision.collider.CompareTag(GameConstants.SHIELD_TAG) && _state is RhinoAttackState)
        {
            KnockBack();
            _isHitShield = true;
            ChangeState(_rhinoWallHitState);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void KnockBack()
    {
        _rb.AddForce( (_isFacingRight) ? _knockBackForce * new Vector2(-1f, 1f) : _knockBackForce, ForceMode2D.Impulse);
    }

}
