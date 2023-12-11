﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : NPCManagers
{
    [Header("Time")]
    [SerializeField] private float _disappearTime;
    [SerializeField] private float _restTime;
    [SerializeField] private float _wanderTime;

    [Header("Speed")]
    [SerializeField] private Vector2 _wanderSpeed;

    [Header("Boundaries")]
    [SerializeField] private Transform _leftBound;
    [SerializeField] private Transform _rightBound;

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
        if (_hasDetectedPlayer && Input.GetKeyDown(KeyCode.T))
            ChangeState(_ghostTalkState);

        _dialog.ToggleIndicator(_hasDetectedPlayer);

        if (!_hasDetectedPlayer)
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

    protected override bool DetectedPlayer()
    {
        return _hasDetectedPlayer = Vector2.Distance(transform.position, _playerRef.position) <= _triggerConversationRange && _state is not GhostDisappearState;
    }

}
