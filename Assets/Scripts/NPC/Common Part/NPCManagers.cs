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

    [Header("Player")]
    [SerializeField] protected Transform _playerRef;

    [Header("Detect Player")] //Dùng cho Raycast phát hiện Player hướng nào để Flip
    [SerializeField] protected Transform _playerCheck;
    [SerializeField] protected LayerMask _playerLayer;

    protected bool _isPlayerNearBy;
    protected bool _hasDetectedPlayer;
    protected Vector2 _conversationPos;

    public float AdjustConversationRange { get { return _adjustConversationRange; } }

    public Transform PlayerRef { get { return _playerRef; } }

    public bool HasDetectedPlayer { get { return _hasDetectedPlayer; } }

    public Vector2 ConversationPos { get { return _conversationPos; } set { _conversationPos = value; } }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        IsPlayerNearBy();
        DetectPlayerByRay();
        DrawLineDetectPlayer();
    }

    protected virtual bool IsPlayerNearBy()
    {
        return _isPlayerNearBy = Mathf.Abs(transform.position.x - _playerRef.position.x) <= _triggerConversationRange;
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
}
