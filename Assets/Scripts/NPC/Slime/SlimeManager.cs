using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeManager : NPCManagers
{
    //Với Slime thì SC bị động là bị Player Jump lên đầu
    //Có thêm "lỗi" là nếu Player Jump đoạn box trigger ngoài rìa thì add 1 lượng force lớn hơn bthg ?

    [SerializeField] private int _startIndexIfGotHit;

    private SlimeTalkState _slimeTalkState = new();
    private SlimeGotHitState _slimeGotHitState = new();
    private bool _hasGotHit;
    //Đánh dấu nếu đã SC theo cách bị động thì lấy index IfGotHit cho Dialog
    private bool _hasStartConversationPassive;

    public int GetStartIndexIfGotHit() { return _startIndexIfGotHit; }

    public bool HasStartConversationPassive { get {  return _hasStartConversationPassive; } set { _hasStartConversationPassive = value; } }

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
            playerScript.GetRigidBody2D().AddForce(playerScript.GetPlayerStats.JumpOnEnemiesForce, ForceMode2D.Impulse);
            ChangeState(_slimeGotHitState);
            _hasStartConversationPassive = true;
        }
    }

    private void BackToIdle()
    {
        ChangeState(_npcIdleState);
        _hasGotHit = false;
        //Event của animation GotHit
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
}
