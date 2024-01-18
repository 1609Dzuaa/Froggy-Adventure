using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManagers : CharactersManager
{
    //Học cách bố cục Dialog cũng như Indicator (Font, ...)
    //Có bug nhỏ khi vừa space kết thúc thoại thì player jump
    //Vẫn còn bug chạy vô định @@

    [Header("Range")]
    [SerializeField] protected float _triggerConversationRange;
    [SerializeField] protected float _adjustConversationRange;
    [SerializeField] protected Vector2 _triggerConvSize;

    [Header("Detect Player"), Tooltip("Dùng cho Raycast phát hiện Player hướng nào để Flip")]
    [SerializeField] protected Transform _playerCheck;
    [SerializeField] protected LayerMask _playerLayer;

    [Header("Dialog")]
    [SerializeField] protected Dialog _dialog;
    [SerializeField] protected int _startIndex; //Chỉ số row trong hộp thoại mà mình muốn bắt đầu

    [Header("Gizmos Radius")]
    [SerializeField] protected float _gizmosRadius;

    [Header("Indicator")]
    [SerializeField] protected bool _needTriggerIndicator;

    protected NPCIdleState _npcIdleState = new();
    protected NPCTalkState _npcTalkState = new();

    protected bool _isPlayerNearBy;
    protected bool _hasDetectedPlayer;
    protected Vector2 _conversationPos;
    protected PlayerStateManager _playerReference;

    public float AdjustConversationRange { get { return _adjustConversationRange; } }

    public NPCIdleState GetNPCIdleState() { return _npcIdleState; }

    public bool HasDetectedPlayer { get { return _hasDetectedPlayer; } }

    public bool NeedTriggerIndicator { set { _needTriggerIndicator = value; } }

    public Vector2 ConversationPos { get { return _conversationPos; } set { _conversationPos = value; } }

    public Dialog GetDialog() { return _dialog; }

    public int GetStartIndex() { return _startIndex; }

    public PlayerStateManager PlayerReference { get => _playerReference; set => _playerReference = value; }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void GetReferenceComponents()
    {
        base.GetReferenceComponents();
        _playerReference = FindObjectOfType<PlayerStateManager>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void SetUpProperties()
    {
        _state = _npcIdleState;
        _state.EnterState(this);
    }

    protected override void Update()
    {
        base.Update();
        IsPlayerNearBy();
        DetectPlayerByRay();
        DrawLineDetectPlayer();
        HandleDialogAndIndicator();
        UpdateConversationPosition();
    }

    protected virtual bool IsPlayerNearBy()
    {
        return _isPlayerNearBy = Physics2D.OverlapBox(transform.position, _triggerConvSize, 0f, _playerLayer) && _playerReference.GetIsOnGround();
    }

    protected virtual void DetectPlayerByRay()
    {
        if (_isFacingRight)
            _hasDetectedPlayer = Physics2D.Raycast(_playerCheck.position, Vector2.right, _triggerConversationRange, _playerLayer);
        else
            _hasDetectedPlayer = Physics2D.Raycast(_playerCheck.position, Vector2.left, _triggerConversationRange, _playerLayer);
    }

    protected void DrawLineDetectPlayer()
    {
        if (_hasDetectedPlayer)
            Debug.DrawLine(transform.position, _playerReference.transform.position, Color.red);
        else
            Debug.DrawLine(transform.position, _playerReference.transform.position, Color.green);
    }

    protected virtual void HandleDialogAndIndicator()
    {
        if (_needTriggerIndicator)
            _dialog.ToggleIndicator(_isPlayerNearBy);

        //Nếu đã bắt đầu Thoại và chưa đến đoạn chờ thì tắt Indicator
        if (_dialog.Started && !_dialog.IsWaiting)
            _dialog.ToggleIndicator(false);

        if (_isPlayerNearBy && Input.GetKeyDown(KeyCode.T) && _state is not NPCTalkState)
        {
            EventsManager.Instance.NotifyObservers(GameEnums.EEvents.PlayerOnInteractWithNPCs, null);
            ChangeState(_npcTalkState);
        }
    }

    protected virtual void UpdateConversationPosition()
    {
        if (_isFacingRight)
            _conversationPos = new Vector2(transform.position.x + _adjustConversationRange, transform.parent.position.y);
        else
            _conversationPos = new Vector2(transform.position.x - _adjustConversationRange, transform.parent.position.y);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawSphere(ConversationPos, _gizmosRadius);
        Gizmos.DrawCube(transform.position, _triggerConvSize);
    }

}
