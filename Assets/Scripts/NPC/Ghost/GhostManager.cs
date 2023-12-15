﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : NPCManagers
{
    //Vẫn còn bug đi quá min @@?
    //Vẫn còn hiện tượng Player né mặt Ghost ? (Do vị trí Player ~ Talk Posistion)
    //Xử lý xong! Về Ghost thì tránh bố trí mấy chỗ vướng, làm kẹt Player

    [Header("Time")]
    [SerializeField] private float _disappearTime;
    [SerializeField] private float _restTime;
    [SerializeField] private float _wanderTime;

    [Header("Speed")]
    [SerializeField] private Vector2 _wanderSpeed;

    [Header("Boundaries")]
    [SerializeField] private Transform _leftBound;
    [SerializeField] private Transform _rightBound;

    [Header("Gizmos Radius")]
    [SerializeField] private float _gizmosRadius;

    [Header("Dialog")]
    [SerializeField] private Dialog _dialog;

    private GhostAppearState _ghostAppearState = new();
    private GhostIdleState _ghostIdleState = new();
    private GhostWanderState _ghostWanderState = new();
    private GhostTalkState _ghostTalkState = new();
    private GhostDisappearState _ghostDisappearState = new();

    public float DisappearTime { get { return _disappearTime; } }

    public float RestTime { get { return _restTime; } }

    public float WanderTime { get { return _wanderTime; } }

    public Vector2 WanderSpeed { get { return _wanderSpeed;} }

    public Transform LeftBound { get { return _leftBound; } }

    public Transform RightBound { get { return _rightBound; } }

    public Dialog GetDialog() { return _dialog; }

    public GhostAppearState GetGhostAppearState() { return _ghostAppearState; }

    public GhostIdleState GetGhostIdleState() { return _ghostIdleState; }

    public GhostWanderState GetGhostWanderState() { return _ghostWanderState; }

    public GhostTalkState GetGhostTalkState() { return _ghostTalkState; }

    public GhostDisappearState GetGhostDisappearState() { return _ghostDisappearState; }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        _state = _ghostAppearState;
        _state.EnterState(this);
    }

    protected override void Update()
    {
        base.Update();
        if (_isFacingRight)
            _conversationPos = new Vector2(transform.position.x + _adjustConversationRange, transform.parent.position.y);
        else
            _conversationPos = new Vector2(transform.position.x - _adjustConversationRange, transform.parent.position.y);

        //Thêm ĐK Player OnGround thì mới cho phép bắt đầu xl
        var playerScript = _playerRef.GetComponent<PlayerStateManager>();
        if (_isPlayerNearBy && Input.GetKeyDown(KeyCode.T) && playerScript.GetIsOnGround())
            ChangeState(_ghostTalkState);

        _dialog.ToggleIndicator(_isPlayerNearBy);

        //Nếu đã bắt đầu Thoại và chưa đến đoạn chờ thì tắt Indicator
        if (_dialog.Started && !_dialog.IsWaiting)
            _dialog.ToggleIndicator(false);

        if (!_isPlayerNearBy)
            _dialog.EndDialog();
        //Debug.Log("Can Talk: " + _hasDetectedPlayer);
    }

    private void FixedUpdate()
    {
        _state.FixedUpdate();
    }

    private void ChangeToIdle()
    {
        ChangeState(_ghostIdleState);
        //Event của animation Appear
    }

    protected override bool IsPlayerNearBy()
    {
        var playerScript = _playerRef.GetComponent<PlayerStateManager>();
        return _isPlayerNearBy = Vector2.Distance(transform.position, _playerRef.position) <= _triggerConversationRange 
            && _state is not GhostDisappearState && playerScript.GetIsOnGround();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(ConversationPos, _gizmosRadius);
    }

}