using UnityEngine;

public class GhostTalkState : CharacterBaseState
{
    private GhostManager _ghostManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _ghostManager = (GhostManager)charactersManager;
        _ghostManager.Animator.SetInteger("state", (int)EnumState.EGhostState.appear);
        _ghostManager.GetRigidbody2D().velocity = Vector2.zero;
        HandleInteractWithPlayer();
        //Debug.Log("Talk");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void Update()
    {
        //Đéo tiếp chuyện nữa thì trả về Idle State
        if (!_ghostManager.GetDialog().Started)
        {
            var playerScript = _ghostManager.PlayerRef.GetComponent<PlayerStateManager>();
            playerScript.IsInteractingWithNPC = false;
            _ghostManager.ChangeState(_ghostManager.GetGhostIdleState());
        }
    }

    private void HandleInteractWithPlayer()
    {
        var playerScript = _ghostManager.PlayerRef.GetComponent<PlayerStateManager>();

        //Tương tự bên Player, Raycast 0 phát hiện Player thì flip ngược lại
        //(Vì chỉ xét 2 hướng trái phải)
        if (!_ghostManager.HasDetectedPlayer)
            _ghostManager.FlippingSprite();

        //Sau khi đã có hướng NPC thì dựa vào đó để xác định vị trí muốn Player đứng để bắt đầu hội thoại
        if (_ghostManager.GetIsFacingRight())
            _ghostManager.ConversationPos = new Vector2(_ghostManager.transform.position.x + _ghostManager.AdjustConversationRange, _ghostManager.transform.parent.position.y);
        else
            _ghostManager.ConversationPos = new Vector2(_ghostManager.transform.position.x - _ghostManager.AdjustConversationRange, _ghostManager.transform.parent.position.y);

        Debug.Log("Ghost Pos: " + _ghostManager.transform.position);
        Debug.Log("pos: " + _ghostManager.ConversationPos);

        //Gán vị trí cần di chuyển cho Player
        playerScript.InteractPosition = _ghostManager.ConversationPos;

        //Đánh dấu đang tương tác với NPC, lock change state
        playerScript.IsInteractingWithNPC = true;

        //Lấy và bắt đầu Thoại
        _ghostManager.GetDialog().StartDialog();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
