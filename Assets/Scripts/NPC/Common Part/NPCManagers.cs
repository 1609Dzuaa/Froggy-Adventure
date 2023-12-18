using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManagers : CharactersManager
{
    //Xử lý thêm nếu Player tiếp chuyện thì tự động bước ra chỗ trước mặt NPC
    //Có vấn đề ở Prefab Dialog

    [Header("Range")]
    [SerializeField] protected float _triggerConversationRange;
    [SerializeField] protected float _adjustConversationRange;

    [Header("Player Reference")]
    [SerializeField] protected Transform _playerRef;

    [Header("Detect Player")] //Dùng cho Raycast phát hiện Player hướng nào để Flip
    [SerializeField] protected Transform _playerCheck;
    [SerializeField] protected LayerMask _playerLayer;

    [Header("Dialog")]
    [SerializeField] protected Dialog _dialog;

    protected NPCIdleState _npcIdleState = new();
    protected NPCTalkState _npcTalkState = new();

    protected bool _isPlayerNearBy;
    protected bool _hasDetectedPlayer;
    protected Vector2 _conversationPos;

    public float AdjustConversationRange { get { return _adjustConversationRange; } }

    public Transform PlayerRef { get { return _playerRef; } }

    public NPCIdleState GetNPCIdleState() { return _npcIdleState; }

    public bool HasDetectedPlayer { get { return _hasDetectedPlayer; } }

    public Vector2 ConversationPos { get { return _conversationPos; } set { _conversationPos = value; } }

    public Dialog GetDialog() { return _dialog; }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
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
    }

    protected virtual bool IsPlayerNearBy()
    {
        var playerScript = _playerRef.GetComponent<PlayerStateManager>();
        return _isPlayerNearBy = Vector2.Distance(transform.position, _playerRef.position) <= _triggerConversationRange
               && playerScript.GetIsOnGround();
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
            Debug.DrawLine(transform.position, _playerRef.position, Color.red);
        else
            Debug.DrawLine(transform.position, _playerRef.position, Color.green);
    }

    protected void HandleDialogAndIndicator()
    {
        _dialog.ToggleIndicator(_isPlayerNearBy);

        //Nếu đã bắt đầu Thoại và chưa đến đoạn chờ thì tắt Indicator
        if (_dialog.Started && !_dialog.IsWaiting)
            _dialog.ToggleIndicator(false);

        if (!_isPlayerNearBy)
            _dialog.EndDialog();
        //Debug.Log("Can Talk: " + _hasDetectedPlayer);

        //Thêm ĐK Player OnGround thì mới cho phép bắt đầu xl
        var playerScript = _playerRef.GetComponent<PlayerStateManager>();
        if (_isPlayerNearBy && Input.GetKeyDown(KeyCode.T) && playerScript.GetIsOnGround())
            ChangeState(_npcTalkState);
    }
}
