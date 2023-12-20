using UnityEngine;

public class DealerTalkState : NPCTalkState
{
    private DealerManager _dealerManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        //_npcManager = (NPCManagers)charactersManager;
        _dealerManager = (DealerManager)charactersManager;
        _npcManager = _dealerManager;
        _dealerManager.Animator.SetInteger("state", (int)EnumState.ESlimState.idle);
        _dealerManager.GetRigidbody2D().velocity = Vector2.zero;

        HandleInteractWithPlayer(_dealerManager, _dealerManager.GetStartIndex());
        Debug.Log("Dealer Talk");
    }

    public override void ExitState()
    {
        //base.ExitState();
        var playerScript = _dealerManager.PlayerRef.GetComponent<PlayerStateManager>();
        playerScript.Enable();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate() { }

    protected override void HandleInteractWithPlayer(NPCManagers npcManagers, int startIndex)
    {
        //Đánh dấu đang tương tác với NPC, lock change state chủ động của Player
        //playerScript.IsInteractingWithNPC = true;

        //Lấy và bắt đầu Thoại
        npcManagers.GetDialog().StartDialog(startIndex);
    }
}
