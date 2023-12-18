using UnityEngine;

public class NPCTalkState : CharacterBaseState
{
    private NPCManagers _npcManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _npcManager = (NPCManagers)charactersManager;
        _npcManager.Animator.SetInteger("state", (int)EnumState.ENPCState.idle);
        _npcManager.GetRigidbody2D().velocity = Vector2.zero;
        HandleInteractWithPlayer(_npcManager);
        Debug.Log("Talk");
    }

    public override void ExitState() { }

    public override void Update()
    {
        //Đéo tiếp chuyện nữa thì trả về Idle State
        if (!_npcManager.GetDialog().Started)
        {
            var playerScript = _npcManager.PlayerRef.GetComponent<PlayerStateManager>();
            playerScript.IsInteractingWithNPC = false;
            _npcManager.ChangeState(_npcManager.GetNPCIdleState());
        }
    }

    protected void HandleInteractWithPlayer(NPCManagers npcManagers)
    {
        var playerScript = npcManagers.PlayerRef.GetComponent<PlayerStateManager>();

        //Tương tự bên Player, Raycast 0 phát hiện Player thì flip ngược lại
        //(Vì chỉ xét 2 hướng trái phải)
        if (!npcManagers.HasDetectedPlayer)
            npcManagers.FlippingSprite();

        //Sau khi đã có hướng NPC thì dựa vào đó để xác định vị trí muốn Player đứng để bắt đầu hội thoại
        if (npcManagers.GetIsFacingRight())
            npcManagers.ConversationPos = new Vector2(npcManagers.transform.position.x + npcManagers.AdjustConversationRange, npcManagers.transform.parent.position.y);
        else
            npcManagers.ConversationPos = new Vector2(npcManagers.transform.position.x - npcManagers.AdjustConversationRange, npcManagers.transform.parent.position.y);

        //Gán vị trí cần di chuyển cho Player
        playerScript.InteractPosition = npcManagers.ConversationPos;

        //Đánh dấu đang tương tác với NPC, lock change state
        playerScript.IsInteractingWithNPC = true;

        //Lấy và bắt đầu Thoại
        npcManagers.GetDialog().StartDialog();
    }

    public override void FixedUpdate() { }
}