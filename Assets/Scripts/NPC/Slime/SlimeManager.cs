using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeManager : NPCManagers
{
    [SerializeField] private int _startIndexIfGotHit;

    private SlimeTalkState _slimeTalkState = new();
    private SlimeGotHitState _slimeGotHitState = new();
    private bool _hasGotHit;
    private bool _hasStartConversation;

    public int GetStartIndexIfGotHit() { return _startIndexIfGotHit; }

    public bool HasStartConversation { get {  return _hasStartConversation; } set { _hasStartConversation = value; } }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        _npcTalkState = _slimeTalkState;
        if (Mathf.Abs(transform.eulerAngles.y) == 180f)
            _isFacingRight = true;
        Debug.Log("IsFR: " + _isFacingRight);
    }

    protected override void Update()
    {
        base.Update();
        HandleFlippingSprite();
    }

    private void HandleFlippingSprite()
    {
        if (transform.position.x >= _playerRef.transform.position.x)
        {
            if (_isFacingRight)
                FlippingSprite();
        }
        else if (transform.position.x < _playerRef.transform.position.x)
        {
            if (!_isFacingRight)
                FlippingSprite();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player" && !_hasGotHit)
        {
            _hasGotHit = true;
            var playerScript = collision.GetComponent<PlayerStateManager>();
            playerScript.SetCanDbJump(true); //Nhảy lên đầu Enemies thì cho phép DbJump tiếp
            playerScript.GetRigidBody2D().AddForce(playerScript.GetJumpOnEnemiesForce(), ForceMode2D.Impulse);
            ChangeState(_slimeGotHitState);
            _hasStartConversation = true;
        }
    }

    private void BackToIdle()
    {
        ChangeState(_npcIdleState);
        _hasGotHit = false;
        //Event của animation GotHit
    }

    protected override void HandleDialogAndIndicator()
    {
        _dialog.ToggleIndicator(_isPlayerNearBy);

        //Nếu đã bắt đầu Thoại và chưa đến đoạn chờ thì tắt Indicator
        if (_dialog.Started && !_dialog.IsWaiting)
            _dialog.ToggleIndicator(false);

        //Kết thúc thoại nếu Player 0 ở gần (rời đi)
        //if (!_isPlayerNearBy)
        //_dialog.EndDialog();
        //Debug.Log("Can Talk: " + _hasDetectedPlayer);

        //Thêm ĐK Player OnGround thì mới cho phép bắt đầu xl
        var playerScript = _playerRef.GetComponent<PlayerStateManager>();
        if (_isPlayerNearBy && Input.GetKeyDown(KeyCode.T) && playerScript.GetIsOnGround())
            ChangeState(_slimeTalkState);
    }
}
