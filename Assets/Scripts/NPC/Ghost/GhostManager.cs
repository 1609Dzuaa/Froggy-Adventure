using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : NPCManagers
{
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
    }

    protected override void SetUpProperties()
    {
        _npcIdleState = _ghostIdleState;
        _npcTalkState = _ghostTalkState;
        base.SetUpProperties();
    }

    protected override void Update()
    {
        base.Update();
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
        return _isPlayerNearBy = Vector2.Distance(transform.position, _playerReference.transform.position) <= _triggerConversationRange 
            && _state is not GhostDisappearState && _playerReference.GetIsOnGround();
    }

}
